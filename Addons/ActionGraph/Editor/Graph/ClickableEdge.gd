extends Control
class_name ClickableEdge


const LINE_WIDTH := 3.0
const CLICK_DISTANCE := 8.0
const SAMPLES := 64


class BezierCurve:
	var start: Vector2
	var end: Vector2

	func _init(s: Vector2, e: Vector2):
		start = s
		end = e


	func get_control_1() -> Vector2:
		var dx : float = abs(end.x - start.x)

		var distance : float = clamp(
			dx * 0.5,
			40.0,
			200.0
		)

		if abs(end.y - start.y) < 0.001:
			return Vector2(
				start.x + (end.x - start.x) * 0.33,
				start.y
			)

		return Vector2(
			start.x + sign(end.x - start.x) * distance,
			start.y
		)


	func get_control_2() -> Vector2:
		var dx : float = abs(end.x - start.x)

		var distance : float = clamp(
			dx * 0.5,
			40.0,
			200.0
		)

		if abs(end.y - start.y) < 0.001:
			return Vector2(
				start.x + (end.x - start.x) * 0.66,
				end.y
			)

		return Vector2(
			end.x - sign(end.x - start.x) * distance,
			end.y
		)



var curves: Array[BezierCurve] = []


func _ready() -> void:
	mouse_filter = Control.MOUSE_FILTER_STOP

	curves.append(
		BezierCurve.new(
			Vector2(100, 300),
			Vector2(700, 100)
		)
	)

	update_bounds()
	queue_redraw()



func update_bounds() -> void:
	if curves.is_empty():
		return

	var min_pos := Vector2(INF, INF)
	var max_pos := Vector2(-INF, -INF)

	for curve in curves:
		var rect := get_curve_bounds(curve)

		min_pos = min_pos.min(rect.position)
		max_pos = max_pos.max(rect.position + rect.size)

	var padding := Vector2.ONE * (CLICK_DISTANCE + LINE_WIDTH)

	position = min_pos - padding
	size = (max_pos - min_pos) + padding * 2



func get_curve_bounds(curve: BezierCurve) -> Rect2:
	var points := sample_curve(curve, SAMPLES)

	var min_pos := Vector2(INF, INF)
	var max_pos := Vector2(-INF, -INF)

	for p in points:
		min_pos = min_pos.min(p)
		max_pos = max_pos.max(p)

	return Rect2(
		min_pos,
		max_pos - min_pos
	)



func _draw() -> void:
	for curve in curves:
		var points := PackedVector2Array()

		for p in sample_curve(curve, SAMPLES):
			points.append(p - position)

		draw_polyline(
			points,
			Color.WHITE,
			LINE_WIDTH
		)



func sample_curve(
	curve: BezierCurve,
	count: int
) -> PackedVector2Array:

	var points := PackedVector2Array()

	var c1 := curve.get_control_1()
	var c2 := curve.get_control_2()

	for i in range(count + 1):
		var t := float(i) / count

		points.append(
			cubic_bezier(
				curve.start,
				c1,
				c2,
				curve.end,
				t
			)
		)

	return points



func cubic_bezier(
	p0: Vector2,
	p1: Vector2,
	p2: Vector2,
	p3: Vector2,
	t: float
) -> Vector2:

	var u := 1.0 - t

	return (
		u * u * u * p0
		+ 3.0 * u * u * t * p1
		+ 3.0 * u * t * t * p2
		+ t * t * t * p3
	)



func _gui_input(event: InputEvent) -> void:
	if event is InputEventMouseButton:

		var mouse_event := event as InputEventMouseButton

		if mouse_event.button_index == MOUSE_BUTTON_LEFT \
		and mouse_event.pressed:

			var mouse := mouse_event.position + position

			for i in range(curves.size()):

				var t := find_click_t(
					curves[i],
					mouse
				)

				if t >= 0.0:

					var split := split_curve(
						curves[i],
						t
					)

					curves.remove_at(i)
					curves.insert(i, split[1])
					curves.insert(i, split[0])

					update_bounds()
					queue_redraw()

					accept_event()
					return



func find_click_t(
	curve: BezierCurve,
	mouse: Vector2
) -> float:

	var points := sample_curve(
		curve,
		SAMPLES
	)

	var best_distance := INF
	var best_t := -1.0

	for i in range(points.size() - 1):

		var closest := Geometry2D.get_closest_point_to_segment(
			mouse,
			points[i],
			points[i + 1]
		)

		var distance := mouse.distance_to(closest)

		if distance < best_distance:

			best_distance = distance

			var segment_length := (
				points[i].distance_to(points[i + 1])
			)

			var local_t := 0.0

			if segment_length > 0:
				local_t = (
					points[i].distance_to(closest)
					/ segment_length
				)

			best_t = lerp(
				float(i) / SAMPLES,
				float(i + 1) / SAMPLES,
				local_t
			)

	if best_distance <= CLICK_DISTANCE:
		return best_t

	return -1.0



func split_curve(
	curve: BezierCurve,
	t: float
) -> Array[BezierCurve]:

	var p0 := curve.start
	var p1 := curve.get_control_1()
	var p2 := curve.get_control_2()
	var p3 := curve.end


	# Cubic De Casteljau

	var p01 := p0.lerp(p1, t)
	var p12 := p1.lerp(p2, t)
	var p23 := p2.lerp(p3, t)

	var p012 := p01.lerp(p12, t)
	var p123 := p12.lerp(p23, t)

	var split := p012.lerp(p123, t)


	return [
		BezierCurve.new(
			p0,
			split
		),
		BezierCurve.new(
			split,
			p3
		)
	]
