////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

#pragma name AGBP
#pragma description ActionGraph Binary Program
#pragma magic [ 41 47 42 50 00 00 00 00 ] @ 0x00

import type.leb128;
import type.color;

using uint = type::uLEB128;
using int = type::sLEB128;
using color = type::RGBA8;

struct string {
    uint length;
    char value[length];
} [[single_color]];

// Base.
struct Element {
    uint index [[color("FFFFFF")]];

    match (index) {
        (0): {
            uint id_length [[color("333333")]];
            char id_text[id_length] [[color("333333")]];
            uint editor_length [[color("707070")]];
            char editor_text[editor_length] [[color("707070")]];
            uint schema_version_length [[color("333333")]];
            char schema_version_text[schema_version_length] [[color("333333")]];
            uint format_version_length [[color("707070")]];
            char format_version_text[format_version_length] [[color("707070")]];
            uint default_language_length [[color("333333")]];
            char default_language_text[default_language_length] [[color("333333")]];
            uint checksum_length [[color("707070")]];
            char checksum_text[checksum_length] [[color("707070")]];
            
            uint child_count [[color("282828")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "file");
        }
        (1): {
            uint id_length [[color("3f8c6d")]];
            char id_text[id_length] [[color("3f8c6d")]];
            
            std::core::set_display_name(this, "language_definition");
        }
        (2): {
            uint id_length [[color("a478cc")]];
            char id_text[id_length] [[color("a478cc")]];
            uint value_length [[color("BFA0DB")]];
            char value_text[value_length] [[color("BFA0DB")]];
            
            uint child_count [[color("8360A3")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "metadata");
        }
        (3): {
            uint id_length [[color("4393d1")]];
            char id_text[id_length] [[color("4393d1")]];
            uint execution_handler_length [[color("7BB3DE")]];
            char execution_handler_text[execution_handler_length] [[color("7BB3DE")]];
            
            uint child_count [[color("3575A7")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "instruction_definition");
        }
        (4): {
            uint id_length [[color("cf7474")]];
            char id_text[id_length] [[color("cf7474")]];
            bool localizable [[color("DD9D9D")]];
            
            std::core::set_display_name(this, "parameter_definition");
        }
        (5): {
            uint id_length [[color("74cf7d")]];
            char id_text[id_length] [[color("74cf7d")]];
            
            uint child_count [[color("5CA564")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "node_definition");
        }
        (6): {
            uint id_length [[color("74b5cf")]];
            char id_text[id_length] [[color("74b5cf")]];
            uint instruction_length [[color("9DCBDD")]];
            char instruction_text[instruction_length] [[color("9DCBDD")]];
            
            uint child_count [[color("5C90A5")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "form_definition");
        }
        (7): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            
            uint child_count [[color("666666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "option_definition");
        }
        (8): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            
            uint child_count [[color("666666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "choice_definition");
        }
        (9): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            
            uint child_count [[color("666666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "tuple_definition");
        }
        (10): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            
            uint child_count [[color("666666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "list_definition");
        }
        (11): {
            uint id_length [[color("f07878")]];
            char id_text[id_length] [[color("f07878")]];
            uint parameter_length [[color("F4A0A0")]];
            char parameter_text[parameter_length] [[color("F4A0A0")]];
            
            std::core::set_display_name(this, "field_definition");
        }
        (12): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            uint parameter_length [[color("A6A6A6")]];
            char parameter_text[parameter_length] [[color("A6A6A6")]];
            bool remove_default_port [[color("808080")]];
            
            std::core::set_display_name(this, "port_definition");
        }
        (13): {
            uint id_length [[color("4bcc4b")]];
            char id_text[id_length] [[color("4bcc4b")]];
            uint definition_length [[color("81DB81")]];
            char definition_text[definition_length] [[color("81DB81")]];
            int x [[color("4bcc4b")]];
            int y [[color("81DB81")]];
            uint width [[color("4bcc4b")]];
            uint height [[color("81DB81")]];
            bool folded [[color("4bcc4b")]];
            uint parent_frame_length [[color("81DB81")]];
            char parent_frame_text[parent_frame_length] [[color("81DB81")]];
            uint text_length [[color("4bcc4b")]];
            char text_text[text_length] [[color("4bcc4b")]];
            u8 color_r [[color("81DB81")]];
            u8 color_g [[color("81DB81")]];
            u8 color_b [[color("81DB81")]];
            u8 color_a [[color("81DB81")]];
            uint start_point_name_length [[color("4bcc4b")]];
            char start_point_name_text[start_point_name_length] [[color("4bcc4b")]];
            
            uint child_count [[color("3CA33C")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "node");
        }
        (14): {
            uint id_length [[color("fff373")]];
            char id_text[id_length] [[color("fff373")]];
            int x [[color("FFF69D")]];
            int y [[color("fff373")]];
            uint width [[color("FFF69D")]];
            uint height [[color("fff373")]];
            bool folded [[color("FFF69D")]];
            uint parent_frame_length [[color("fff373")]];
            char parent_frame_text[parent_frame_length] [[color("fff373")]];
            uint text_length [[color("FFF69D")]];
            char text_text[text_length] [[color("FFF69D")]];
            u8 color_r [[color("fff373")]];
            u8 color_g [[color("fff373")]];
            u8 color_b [[color("fff373")]];
            u8 color_a [[color("fff373")]];
            
            std::core::set_display_name(this, "memo");
        }
        (15): {
            uint id_length [[color("8068f6")]];
            char id_text[id_length] [[color("8068f6")]];
            int x [[color("A695F8")]];
            int y [[color("8068f6")]];
            uint width [[color("A695F8")]];
            uint height [[color("8068f6")]];
            bool folded [[color("A695F8")]];
            uint parent_frame_length [[color("8068f6")]];
            char parent_frame_text[parent_frame_length] [[color("8068f6")]];
            uint text_length [[color("A695F8")]];
            char text_text[text_length] [[color("A695F8")]];
            u8 color_r [[color("8068f6")]];
            u8 color_g [[color("8068f6")]];
            u8 color_b [[color("8068f6")]];
            u8 color_a [[color("8068f6")]];
            
            std::core::set_display_name(this, "frame");
        }
        (16): {
            uint id_length [[color("bee0ed")]];
            char id_text[id_length] [[color("bee0ed")]];
            uint edge_length [[color("D1E9F2")]];
            char edge_text[edge_length] [[color("D1E9F2")]];
            int x [[color("bee0ed")]];
            int y [[color("D1E9F2")]];
            uint size [[color("bee0ed")]];
            uint parent_frame_length [[color("D1E9F2")]];
            char parent_frame_text[parent_frame_length] [[color("D1E9F2")]];
            uint text_length [[color("bee0ed")]];
            char text_text[text_length] [[color("bee0ed")]];
            u8 color_r [[color("D1E9F2")]];
            u8 color_g [[color("D1E9F2")]];
            u8 color_b [[color("D1E9F2")]];
            u8 color_a [[color("D1E9F2")]];
            
            std::core::set_display_name(this, "joint");
        }
        (17): {
            uint id_length [[color("3646f5")]];
            char id_text[id_length] [[color("3646f5")]];
            uint from_length [[color("727DF8")]];
            char from_text[from_length] [[color("727DF8")]];
            uint port [[color("3646f5")]];
            uint to_length [[color("727DF8")]];
            char to_text[to_length] [[color("727DF8")]];
            uint thickness [[color("3646f5")]];
            uint text_length [[color("727DF8")]];
            char text_text[text_length] [[color("727DF8")]];
            u8 color_r [[color("3646f5")]];
            u8 color_g [[color("3646f5")]];
            u8 color_b [[color("3646f5")]];
            u8 color_a [[color("3646f5")]];
            
            std::core::set_display_name(this, "edge");
        }
        (18): {
            uint id_length [[color("13ace9")]];
            char id_text[id_length] [[color("13ace9")]];
            uint definition_length [[color("59C4EF")]];
            char definition_text[definition_length] [[color("59C4EF")]];
            bool folded [[color("13ace9")]];
            uint text_length [[color("59C4EF")]];
            char text_text[text_length] [[color("59C4EF")]];
            u8 color_r [[color("13ace9")]];
            u8 color_g [[color("13ace9")]];
            u8 color_b [[color("13ace9")]];
            u8 color_a [[color("13ace9")]];
            
            uint child_count [[color("0F89BA")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "form");
        }
        (19): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            uint definition_length [[color("A6A6A6")]];
            char definition_text[definition_length] [[color("A6A6A6")]];
            bool enabled [[color("808080")]];
            bool folded [[color("A6A6A6")]];
            uint text_length [[color("808080")]];
            char text_text[text_length] [[color("808080")]];
            u8 color_r [[color("A6A6A6")]];
            u8 color_g [[color("A6A6A6")]];
            u8 color_b [[color("A6A6A6")]];
            u8 color_a [[color("A6A6A6")]];
            
            uint child_count [[color("666666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "option");
        }
        (20): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            uint definition_length [[color("A6A6A6")]];
            char definition_text[definition_length] [[color("A6A6A6")]];
            uint selected_length [[color("808080")]];
            char selected_text[selected_length] [[color("808080")]];
            bool folded [[color("A6A6A6")]];
            uint text_length [[color("808080")]];
            char text_text[text_length] [[color("808080")]];
            u8 color_r [[color("A6A6A6")]];
            u8 color_g [[color("A6A6A6")]];
            u8 color_b [[color("A6A6A6")]];
            u8 color_a [[color("A6A6A6")]];
            
            uint child_count [[color("666666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "choice");
        }
        (21): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            uint definition_length [[color("A6A6A6")]];
            char definition_text[definition_length] [[color("A6A6A6")]];
            bool folded [[color("808080")]];
            uint text_length [[color("A6A6A6")]];
            char text_text[text_length] [[color("A6A6A6")]];
            u8 color_r [[color("808080")]];
            u8 color_g [[color("808080")]];
            u8 color_b [[color("808080")]];
            u8 color_a [[color("808080")]];
            
            uint child_count [[color("666666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "tuple");
        }
        (22): {
            uint id_length [[color("808080")]];
            char id_text[id_length] [[color("808080")]];
            uint definition_length [[color("A6A6A6")]];
            char definition_text[definition_length] [[color("A6A6A6")]];
            bool folded [[color("808080")]];
            uint text_length [[color("A6A6A6")]];
            char text_text[text_length] [[color("A6A6A6")]];
            u8 color_r [[color("808080")]];
            u8 color_g [[color("808080")]];
            u8 color_b [[color("808080")]];
            u8 color_a [[color("808080")]];
            
            uint child_count [[color("666666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "list");
        }
        (23): {
            uint id_length [[color("f00000")]];
            char id_text[id_length] [[color("f00000")]];
            uint definition_length [[color("F44C4C")]];
            char definition_text[definition_length] [[color("F44C4C")]];
            uint value_length [[color("f00000")]];
            char value_text[value_length] [[color("f00000")]];
            uint text_length [[color("F44C4C")]];
            char text_text[text_length] [[color("F44C4C")]];
            u8 color_r [[color("f00000")]];
            u8 color_g [[color("f00000")]];
            u8 color_b [[color("f00000")]];
            u8 color_a [[color("f00000")]];
            
            uint child_count [[color("C00000")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "field");
        }
        (24): {
            uint id_length [[color("f07400")]];
            char id_text[id_length] [[color("f07400")]];
            uint definition_length [[color("F49D4C")]];
            char definition_text[definition_length] [[color("F49D4C")]];
            uint index [[color("f07400")]];
            uint text_length [[color("F49D4C")]];
            char text_text[text_length] [[color("F49D4C")]];
            u8 color_r [[color("f07400")]];
            u8 color_g [[color("f07400")]];
            u8 color_b [[color("f07400")]];
            u8 color_a [[color("f07400")]];
            
            std::core::set_display_name(this, "port");
        }
        (25): {
            uint id_length [[color("d6866b")]];
            char id_text[id_length] [[color("d6866b")]];
            uint language_length [[color("E2AA97")]];
            char language_text[language_length] [[color("E2AA97")]];
            uint value_length [[color("d6866b")]];
            char value_text[value_length] [[color("d6866b")]];
            
            std::core::set_display_name(this, "localized_value");
        }
    }
};

// Entry point.
struct Root {
    char magic_header[8] [[color("000000")]];
    uint version_length [[color("333333")]];
    char version[version_length] [[color("333333")]];
    Element element;
};

Root root @ 0x00;
