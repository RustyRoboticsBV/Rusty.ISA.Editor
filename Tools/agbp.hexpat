////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

import type.leb128;
import type.color;
import std.string;

using uint = type::uLEB128;
using int = type::sLEB128;
using color = type::RGBA8;

struct string {
    uint length;
    char value[length];
};

struct Element {
    uint index;
    string id;
};

struct File : Element {
    string editor;
    string schema_version;
    string format_version;
    string default_language;
    string checksum;
    
    uint child_count;
    Element children[child_count];
};

struct LanguageDefinition : Element {
};

struct Metadata : Element {
    string value;
    
    uint child_count;
    Element children[child_count];
};

struct InstructionDefinition : Element {
    string execution_handler;
    
    uint child_count;
    Element children[child_count];
};

struct ParameterDefinition : Element {
    bool localizable;
};

struct NodeDefinition : Element {
    
    uint child_count;
    Element children[child_count];
};

struct FormDefinition : Element {
    string instruction;
    
    uint child_count;
    Element children[child_count];
};

struct OptionDefinition : Element {
    
    uint child_count;
    Element children[child_count];
};

struct ChoiceDefinition : Element {
    
    uint child_count;
    Element children[child_count];
};

struct TupleDefinition : Element {
    
    uint child_count;
    Element children[child_count];
};

struct ListDefinition : Element {
    
    uint child_count;
    Element children[child_count];
};

struct FieldDefinition : Element {
    string parameter;
};

struct PortDefinition : Element {
    string parameter;
    bool remove_default_port;
};

struct Node : Element {
    string definition;
    int x;
    int y;
    uint width;
    uint height;
    bool folded;
    string parent_frame;
    string text;
    color color;
    string start_point_name;
    
    uint child_count;
    Element children[child_count];
};

struct Memo : Element {
    int x;
    int y;
    uint width;
    uint height;
    bool folded;
    string parent_frame;
    string text;
    color color;
};

struct Frame : Element {
    int x;
    int y;
    uint width;
    uint height;
    bool folded;
    string parent_frame;
    string text;
    color color;
};

struct Joint : Element {
    string edge;
    int x;
    int y;
    uint size;
    string parent_frame;
    string text;
    color color;
};

struct Edge : Element {
    string _from;
    uint port;
    string to;
    uint thickness;
    string text;
    color color;
};

struct Form : Element {
    string definition;
    bool folded;
    string text;
    color color;
    
    uint child_count;
    Element children[child_count];
};

struct Option : Element {
    string definition;
    bool enabled;
    bool folded;
    string text;
    color color;
    
    uint child_count;
    Element children[child_count];
};

struct Choice : Element {
    string definition;
    string selected;
    bool folded;
    string text;
    color color;
    
    uint child_count;
    Element children[child_count];
};

struct Tuple : Element {
    string definition;
    bool folded;
    string text;
    color color;
    
    uint child_count;
    Element children[child_count];
};

struct List : Element {
    string definition;
    bool folded;
    string text;
    color color;
    
    uint child_count;
    Element children[child_count];
};

struct Field : Element {
    string definition;
    string value;
    string text;
    color color;
    
    uint child_count;
    Element children[child_count];
};

struct Port : Element {
    string definition;
    uint index;
    string text;
    color color;
};

struct LocalizedValue : Element {
    string language;
    string value;
};

LocalizedValue root @ 0x00;
