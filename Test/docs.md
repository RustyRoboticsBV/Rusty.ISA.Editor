# Codec Reference

## 0x00 - `file`

| Codec hex | Codec name |
| --- | --- |
| `0x00` | `file` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>editor</code></td><td><code>string</code></td></tr>
| <tr><td>0x02</td><td><code>csum</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>meta</code></li>
| <li><code>lang</code></li>
| <li><code>idef</code></li>
| <li><code>ndef</code></li>
| <li><code>node</code></li>
| <li><code>joint</code></li>
| <li><code>frame</code></li>
| <li><code>memo</code></li>
| <li><code>edge</code></li>
| </ul>

## 0x01 - `meta`

| Codec hex | Codec name |
| --- | --- |
| `0x01` | `meta` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>value</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x02 - `lang`

| Codec hex | Codec name |
| --- | --- |
| `0x02` | `lang` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x03 - `idef`

| Codec hex | Codec name |
| --- | --- |
| `0x03` | `idef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>exec</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>pdef</code></li>
| </ul>

## 0x04 - `pdef`

| Codec hex | Codec name |
| --- | --- |
| `0x04` | `pdef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>loc</code></td><td><code>bool</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x05 - `ndef`

| Codec hex | Codec name |
| --- | --- |
| `0x05` | `ndef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>fdef</code></li>
| <li><code>odef</code></li>
| <li><code>cdef</code></li>
| <li><code>tdef</code></li>
| <li><code>ldef</code></li>
| </ul>

## 0x06 - `fdef`

| Codec hex | Codec name |
| --- | --- |
| `0x06` | `fdef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>vdef</code></li>
| <li><code>jdef</code></li>
| </ul>

## 0x07 - `odef`

| Codec hex | Codec name |
| --- | --- |
| `0x07` | `odef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>fdef</code></li>
| <li><code>odef</code></li>
| <li><code>cdef</code></li>
| <li><code>tdef</code></li>
| <li><code>ldef</code></li>
| </ul>

## 0x08 - `cdef`

| Codec hex | Codec name |
| --- | --- |
| `0x08` | `cdef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>fdef</code></li>
| <li><code>odef</code></li>
| <li><code>cdef</code></li>
| <li><code>tdef</code></li>
| <li><code>ldef</code></li>
| </ul>

## 0x09 - `tdef`

| Codec hex | Codec name |
| --- | --- |
| `0x09` | `tdef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>fdef</code></li>
| <li><code>odef</code></li>
| <li><code>cdef</code></li>
| <li><code>tdef</code></li>
| <li><code>ldef</code></li>
| </ul>

## 0x0A - `ldef`

| Codec hex | Codec name |
| --- | --- |
| `0x0A` | `ldef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>fdef</code></li>
| <li><code>odef</code></li>
| <li><code>cdef</code></li>
| <li><code>tdef</code></li>
| <li><code>ldef</code></li>
| </ul>

## 0x0B - `vdef`

| Codec hex | Codec name |
| --- | --- |
| `0x0B` | `vdef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x0C - `jdef`

| Codec hex | Codec name |
| --- | --- |
| `0x0C` | `jdef` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| <tr><td>0x02</td><td><code>nodflt</code></td><td><code>bool</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x0D - `node`

| Codec hex | Codec name |
| --- | --- |
| `0x0D` | `node` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| <tr><td>0x02</td><td><code>x</code></td><td><code>int</code></td></tr>
| <tr><td>0x03</td><td><code>y</code></td><td><code>int</code></td></tr>
| <tr><td>0x04</td><td><code>width</code></td><td><code>uint</code></td></tr>
| <tr><td>0x05</td><td><code>height</code></td><td><code>uint</code></td></tr>
| <tr><td>0x06</td><td><code>folded</code></td><td><code>bool</code></td></tr>
| <tr><td>0x07</td><td><code>member</code></td><td><code>string</code></td></tr>
| <tr><td>0x08</td><td><code>start</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>form</code></li>
| <li><code>option</code></li>
| <li><code>choice</code></li>
| <li><code>tuple</code></li>
| <li><code>list</code></li>
| </ul>

## 0x0E - `joint`

| Codec hex | Codec name |
| --- | --- |
| `0x0E` | `joint` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>x</code></td><td><code>int</code></td></tr>
| <tr><td>0x02</td><td><code>y</code></td><td><code>int</code></td></tr>
| <tr><td>0x03</td><td><code>member</code></td><td><code>string</code></td></tr>
| <tr><td>0x04</td><td><code>edge</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x0F - `frame`

| Codec hex | Codec name |
| --- | --- |
| `0x0F` | `frame` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>x</code></td><td><code>int</code></td></tr>
| <tr><td>0x02</td><td><code>y</code></td><td><code>int</code></td></tr>
| <tr><td>0x03</td><td><code>width</code></td><td><code>uint</code></td></tr>
| <tr><td>0x04</td><td><code>height</code></td><td><code>uint</code></td></tr>
| <tr><td>0x05</td><td><code>folded</code></td><td><code>bool</code></td></tr>
| <tr><td>0x06</td><td><code>member</code></td><td><code>string</code></td></tr>
| <tr><td>0x07</td><td><code>text</code></td><td><code>string</code></td></tr>
| <tr><td>0x08</td><td><code>color</code></td><td><code>color</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x10 - `memo`

| Codec hex | Codec name |
| --- | --- |
| `0x10` | `memo` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>x</code></td><td><code>int</code></td></tr>
| <tr><td>0x02</td><td><code>y</code></td><td><code>int</code></td></tr>
| <tr><td>0x03</td><td><code>width</code></td><td><code>uint</code></td></tr>
| <tr><td>0x04</td><td><code>height</code></td><td><code>uint</code></td></tr>
| <tr><td>0x05</td><td><code>folded</code></td><td><code>bool</code></td></tr>
| <tr><td>0x06</td><td><code>member</code></td><td><code>string</code></td></tr>
| <tr><td>0x07</td><td><code>text</code></td><td><code>string</code></td></tr>
| <tr><td>0x08</td><td><code>color</code></td><td><code>color</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x11 - `edge`

| Codec hex | Codec name |
| --- | --- |
| `0x11` | `edge` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>from</code></td><td><code>string</code></td></tr>
| <tr><td>0x02</td><td><code>port</code></td><td><code>uint</code></td></tr>
| <tr><td>0x03</td><td><code>to</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x12 - `form`

| Codec hex | Codec name |
| --- | --- |
| `0x12` | `form` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>arg</code></li>
| <li><code>out</code></li>
| </ul>

## 0x13 - `option`

| Codec hex | Codec name |
| --- | --- |
| `0x13` | `option` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>form</code></li>
| <li><code>option</code></li>
| <li><code>choice</code></li>
| <li><code>tuple</code></li>
| <li><code>list</code></li>
| </ul>

## 0x14 - `choice`

| Codec hex | Codec name |
| --- | --- |
| `0x14` | `choice` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| <tr><td>0x02</td><td><code>index</code></td><td><code>uint</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>form</code></li>
| <li><code>option</code></li>
| <li><code>choice</code></li>
| <li><code>tuple</code></li>
| <li><code>list</code></li>
| </ul>

## 0x15 - `tuple`

| Codec hex | Codec name |
| --- | --- |
| `0x15` | `tuple` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>form</code></li>
| <li><code>option</code></li>
| <li><code>choice</code></li>
| <li><code>tuple</code></li>
| <li><code>list</code></li>
| </ul>

## 0x16 - `list`

| Codec hex | Codec name |
| --- | --- |
| `0x16` | `list` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>form</code></li>
| <li><code>option</code></li>
| <li><code>choice</code></li>
| <li><code>tuple</code></li>
| <li><code>list</code></li>
| </ul>

## 0x17 - `arg`

| Codec hex | Codec name |
| --- | --- |
| `0x17` | `arg` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| <tr><td>0x02</td><td><code>value</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li><code>loc</code></li>
| </ul>

## 0x18 - `loc`

| Codec hex | Codec name |
| --- | --- |
| `0x18` | `loc` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| <tr><td>0x02</td><td><code>value</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

## 0x19 - `out`

| Codec hex | Codec name |
| --- | --- |
| `0x19` | `out` |
| Attributes |
| --- |
| <table>
| <thead><tr><th>Hex</th><th>Name</th><th>Type</th></tr></thead>
| <tbody>
| <tr><td>0x00</td><td><code>id</code></td><td><code>string</code></td></tr>
| <tr><td>0x01</td><td><code>type</code></td><td><code>string</code></td></tr>
| </tbody>
| </table>
| Children |
| --- |
| <ul>
| <li>None</li>
| </ul>

