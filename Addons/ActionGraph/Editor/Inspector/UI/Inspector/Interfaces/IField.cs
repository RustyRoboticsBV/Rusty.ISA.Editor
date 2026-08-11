using Godot;

namespace Rusty.ActionGraph.Editor;

internal interface IField
{
    /* Public properties. */
    public string TitleText { get; set; }
    public int TitleWidth { get; set; }
    public string TooltipText { get; set; }
    public UndoRedo UndoRedo { get; set; }
    
    /* Public methods. */
    /// <summary>
    /// Duplicate this field.
    /// </summary>
    public IField DuplicateField();
}

internal interface IField<T> : IField
    where T : IField
{
    /* Public methods. */
    IField IField.DuplicateField() => DuplicateField();

    /// <summary>
    /// Duplicate this field.
    /// </summary>
    public new T DuplicateField();
}