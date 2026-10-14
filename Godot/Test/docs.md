# Codec Reference

### `0x00 (file)`
| Property | Value(s) |
| --- | --- |
| Index | 0 (`0x00`) |
| Name | `file` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>editor</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>csum</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `meta`, `lang`, `idef`, `ndef`, `node`, `joint`, `frame`, `memo`, `edge` |

### `0x01 (meta)`
| Property | Value(s) |
| --- | --- |
| Index | 1 (`0x01`) |
| Name | `meta` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>value</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x02 (lang)`
| Property | Value(s) |
| --- | --- |
| Index | 2 (`0x02`) |
| Name | `lang` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x03 (idef)`
| Property | Value(s) |
| --- | --- |
| Index | 3 (`0x03`) |
| Name | `idef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>exec</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `pdef` |

### `0x04 (pdef)`
| Property | Value(s) |
| --- | --- |
| Index | 4 (`0x04`) |
| Name | `pdef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>loc</code></td><td><code>bool</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x05 (ndef)`
| Property | Value(s) |
| --- | --- |
| Index | 5 (`0x05`) |
| Name | `ndef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |

### `0x06 (fdef)`
| Property | Value(s) |
| --- | --- |
| Index | 6 (`0x06`) |
| Name | `fdef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `vdef`, `jdef` |

### `0x07 (odef)`
| Property | Value(s) |
| --- | --- |
| Index | 7 (`0x07`) |
| Name | `odef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |

### `0x08 (cdef)`
| Property | Value(s) |
| --- | --- |
| Index | 8 (`0x08`) |
| Name | `cdef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |

### `0x09 (tdef)`
| Property | Value(s) |
| --- | --- |
| Index | 9 (`0x09`) |
| Name | `tdef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |

### `0x0A (ldef)`
| Property | Value(s) |
| --- | --- |
| Index | 10 (`0x0A`) |
| Name | `ldef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `fdef`, `odef`, `cdef`, `tdef`, `ldef` |

### `0x0B (vdef)`
| Property | Value(s) |
| --- | --- |
| Index | 11 (`0x0B`) |
| Name | `vdef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x0C (jdef)`
| Property | Value(s) |
| --- | --- |
| Index | 12 (`0x0C`) |
| Name | `jdef` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>nodflt</code></td><td><code>bool</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x0D (node)`
| Property | Value(s) |
| --- | --- |
| Index | 13 (`0x0D`) |
| Name | `node` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>x</code></td><td><code>int</code></td><td><code>?????</code></td></tr><tr><td><code>0x03</code></td><td><code>y</code></td><td><code>int</code></td><td><code>?????</code></td></tr><tr><td><code>0x04</code></td><td><code>width</code></td><td><code>uint</code></td><td><code>?????</code></td></tr><tr><td><code>0x05</code></td><td><code>height</code></td><td><code>uint</code></td><td><code>?????</code></td></tr><tr><td><code>0x06</code></td><td><code>folded</code></td><td><code>bool</code></td><td><code>?????</code></td></tr><tr><td><code>0x07</code></td><td><code>member</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x08</code></td><td><code>start</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `form`, `option`, `choice`, `tuple`, `list` |

### `0x0E (joint)`
| Property | Value(s) |
| --- | --- |
| Index | 14 (`0x0E`) |
| Name | `joint` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>x</code></td><td><code>int</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>y</code></td><td><code>int</code></td><td><code>?????</code></td></tr><tr><td><code>0x03</code></td><td><code>member</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x04</code></td><td><code>edge</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x0F (frame)`
| Property | Value(s) |
| --- | --- |
| Index | 15 (`0x0F`) |
| Name | `frame` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>x</code></td><td><code>int</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>y</code></td><td><code>int</code></td><td><code>?????</code></td></tr><tr><td><code>0x03</code></td><td><code>width</code></td><td><code>uint</code></td><td><code>?????</code></td></tr><tr><td><code>0x04</code></td><td><code>height</code></td><td><code>uint</code></td><td><code>?????</code></td></tr><tr><td><code>0x05</code></td><td><code>folded</code></td><td><code>bool</code></td><td><code>?????</code></td></tr><tr><td><code>0x06</code></td><td><code>member</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x07</code></td><td><code>text</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x08</code></td><td><code>color</code></td><td><code>color</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x10 (memo)`
| Property | Value(s) |
| --- | --- |
| Index | 16 (`0x10`) |
| Name | `memo` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>x</code></td><td><code>int</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>y</code></td><td><code>int</code></td><td><code>?????</code></td></tr><tr><td><code>0x03</code></td><td><code>width</code></td><td><code>uint</code></td><td><code>?????</code></td></tr><tr><td><code>0x04</code></td><td><code>height</code></td><td><code>uint</code></td><td><code>?????</code></td></tr><tr><td><code>0x05</code></td><td><code>folded</code></td><td><code>bool</code></td><td><code>?????</code></td></tr><tr><td><code>0x06</code></td><td><code>member</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x07</code></td><td><code>text</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x08</code></td><td><code>color</code></td><td><code>color</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x11 (edge)`
| Property | Value(s) |
| --- | --- |
| Index | 17 (`0x11`) |
| Name | `edge` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>from</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>port</code></td><td><code>uint</code></td><td><code>?????</code></td></tr><tr><td><code>0x03</code></td><td><code>to</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x12 (form)`
| Property | Value(s) |
| --- | --- |
| Index | 18 (`0x12`) |
| Name | `form` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `arg`, `out` |

### `0x13 (option)`
| Property | Value(s) |
| --- | --- |
| Index | 19 (`0x13`) |
| Name | `option` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `form`, `option`, `choice`, `tuple`, `list` |

### `0x14 (choice)`
| Property | Value(s) |
| --- | --- |
| Index | 20 (`0x14`) |
| Name | `choice` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>index</code></td><td><code>uint</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `form`, `option`, `choice`, `tuple`, `list` |

### `0x15 (tuple)`
| Property | Value(s) |
| --- | --- |
| Index | 21 (`0x15`) |
| Name | `tuple` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `form`, `option`, `choice`, `tuple`, `list` |

### `0x16 (list)`
| Property | Value(s) |
| --- | --- |
| Index | 22 (`0x16`) |
| Name | `list` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `form`, `option`, `choice`, `tuple`, `list` |

### `0x17 (arg)`
| Property | Value(s) |
| --- | --- |
| Index | 23 (`0x17`) |
| Name | `arg` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>value</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | `loc` |

### `0x18 (loc)`
| Property | Value(s) |
| --- | --- |
| Index | 24 (`0x18`) |
| Name | `loc` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x02</code></td><td><code>value</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

### `0x19 (out)`
| Property | Value(s) |
| --- | --- |
| Index | 25 (`0x19`) |
| Name | `out` |
| Description | .................... |
| Attributes | <table><thead><tr><th>Hex</th><th>Name</th><th>Type</th><th>Description</th></tr></thead><tbody><tr><td><code>0x00</code></td><td><code>id</code></td><td><code>string</code></td><td><code>?????</code></td></tr><tr><td><code>0x01</code></td><td><code>type</code></td><td><code>string</code></td><td><code>?????</code></td></tr></tbody></table> |
| Allowed children | *None* |

