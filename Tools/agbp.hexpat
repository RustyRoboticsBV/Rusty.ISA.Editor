////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated file. It should not be edited directly.                //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

#pragma name AGBP
#pragma description ActionGraph Binary Program
#pragma magic [ 41 47 42 50 00 00 00 00 ] @ 0x00

import type.leb128;
import std.core;

using uint = type::uLEB128;
using int = type::sLEB128;

// Base.
struct Element {
    uint index [[color("FFFFFF")]];

    match (index) {
        (0): {
            uint id_length [[color("333330")]];
            char id_text[id_length] [[color("333330")]];
            uint editor_length [[color("70706E")]];
            char editor_text[editor_length] [[color("70706E")]];
            uint schema_version_length [[color("333330")]];
            char schema_version_text[schema_version_length] [[color("333330")]];
            uint default_language_length [[color("70706E")]];
            char default_language_text[default_language_length] [[color("70706E")]];
            uint checksum_length [[color("333330")]];
            char checksum_text[checksum_length] [[color("333330")]];
            
            uint child_count [[color("282826")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "file");
        }
        (1): {
            uint id_length [[color("ffd400")]];
            char id_text[id_length] [[color("ffd400")]];
            
            std::core::set_display_name(this, "language_definition");
        }
        (2): {
            uint id_length [[color("814a08")]];
            char id_text[id_length] [[color("814a08")]];
            uint value_length [[color("A68052")]];
            char value_text[value_length] [[color("A68052")]];
            
            uint child_count [[color("673B06")]];
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
            uint id_length [[color("ba9d95")]];
            char id_text[id_length] [[color("ba9d95")]];
            
            uint child_count [[color("947D77")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "option_definition");
        }
        (7): {
            uint id_length [[color("b4ba95")]];
            char id_text[id_length] [[color("b4ba95")]];
            
            uint child_count [[color("909477")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "choice_definition");
        }
        (8): {
            uint id_length [[color("95baa5")]];
            char id_text[id_length] [[color("95baa5")]];
            
            uint child_count [[color("779484")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "tuple_definition");
        }
        (9): {
            uint id_length [[color("95b0ba")]];
            char id_text[id_length] [[color("95b0ba")]];
            
            uint child_count [[color("778C94")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "list_definition");
        }
        (10): {
            uint id_length [[color("74ede9")]];
            char id_text[id_length] [[color("74ede9")]];
            uint instruction_length [[color("9DF2EF")]];
            char instruction_text[instruction_length] [[color("9DF2EF")]];
            
            uint child_count [[color("5CBDBA")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "form_definition");
        }
        (11): {
            uint id_length [[color("e16397")]];
            char id_text[id_length] [[color("e16397")]];
            uint parameter_length [[color("EA91B6")]];
            char parameter_text[parameter_length] [[color("EA91B6")]];
            bool localizable [[color("e16397")]];
            
            std::core::set_display_name(this, "field_definition");
        }
        (12): {
            uint id_length [[color("ae74cf")]];
            char id_text[id_length] [[color("ae74cf")]];
            uint parameter_length [[color("C69DDD")]];
            char parameter_text[parameter_length] [[color("C69DDD")]];
            bool remove_default_port [[color("ae74cf")]];
            
            std::core::set_display_name(this, "port_definition");
        }
        (13): {
            uint id_length [[color("333330")]];
            char id_text[id_length] [[color("333330")]];
            uint text_length [[color("70706E")]];
            char text_text[text_length] [[color("70706E")]];
            
            uint child_count [[color("282826")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "subgraph_definition");
        }
        (14): {
            uint id_length [[color("000000")]];
            char id_text[id_length] [[color("000000")]];
            int x [[color("4C4C4C")]];
            int y [[color("000000")]];
            uint width [[color("4C4C4C")]];
            uint height [[color("000000")]];
            bool folded [[color("4C4C4C")]];
            
            std::core::set_display_name(this, "start_point");
        }
        (15): {
            uint id_length [[color("67e527")]];
            char id_text[id_length] [[color("67e527")]];
            uint definition_length [[color("94EC67")]];
            char definition_text[definition_length] [[color("94EC67")]];
            int x [[color("67e527")]];
            int y [[color("94EC67")]];
            uint width [[color("67e527")]];
            uint height [[color("94EC67")]];
            bool folded [[color("67e527")]];
            uint parent_frame_length [[color("94EC67")]];
            char parent_frame_text[parent_frame_length] [[color("94EC67")]];
            
            uint child_count [[color("52B71F")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "node");
        }
        (16): {
            uint id_length [[color("ba9d95")]];
            char id_text[id_length] [[color("ba9d95")]];
            uint definition_length [[color("CEBAB4")]];
            char definition_text[definition_length] [[color("CEBAB4")]];
            bool enabled [[color("ba9d95")]];
            bool folded [[color("CEBAB4")]];
            
            uint child_count [[color("947D77")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "option");
        }
        (17): {
            uint id_length [[color("b4ba95")]];
            char id_text[id_length] [[color("b4ba95")]];
            uint definition_length [[color("CACEB4")]];
            char definition_text[definition_length] [[color("CACEB4")]];
            uint selected_length [[color("b4ba95")]];
            char selected_text[selected_length] [[color("b4ba95")]];
            bool folded [[color("CACEB4")]];
            
            uint child_count [[color("909477")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "choice");
        }
        (18): {
            uint id_length [[color("95baa5")]];
            char id_text[id_length] [[color("95baa5")]];
            uint definition_length [[color("B4CEC0")]];
            char definition_text[definition_length] [[color("B4CEC0")]];
            bool folded [[color("95baa5")]];
            
            uint child_count [[color("779484")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "tuple");
        }
        (19): {
            uint id_length [[color("95b0ba")]];
            char id_text[id_length] [[color("95b0ba")]];
            uint definition_length [[color("B4C7CE")]];
            char definition_text[definition_length] [[color("B4C7CE")]];
            bool folded [[color("95b0ba")]];
            
            uint child_count [[color("778C94")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "list");
        }
        (20): {
            uint id_length [[color("13ace9")]];
            char id_text[id_length] [[color("13ace9")]];
            uint definition_length [[color("59C4EF")]];
            char definition_text[definition_length] [[color("59C4EF")]];
            bool folded [[color("13ace9")]];
            
            uint child_count [[color("0F89BA")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "form");
        }
        (21): {
            uint id_length [[color("f00000")]];
            char id_text[id_length] [[color("f00000")]];
            uint definition_length [[color("F44C4C")]];
            char definition_text[definition_length] [[color("F44C4C")]];
            uint value_length [[color("f00000")]];
            char value_text[value_length] [[color("f00000")]];
            
            uint child_count [[color("C00000")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "field");
        }
        (22): {
            uint id_length [[color("d6bc6b")]];
            char id_text[id_length] [[color("d6bc6b")]];
            uint language_length [[color("E2D097")]];
            char language_text[language_length] [[color("E2D097")]];
            uint value_length [[color("d6bc6b")]];
            char value_text[value_length] [[color("d6bc6b")]];
            
            std::core::set_display_name(this, "localized_value");
        }
        (23): {
            uint id_length [[color("f07400")]];
            char id_text[id_length] [[color("f07400")]];
            uint definition_length [[color("F49D4C")]];
            char definition_text[definition_length] [[color("F49D4C")]];
            uint index [[color("f07400")]];
            
            std::core::set_display_name(this, "port");
        }
        (24): {
            uint id_length [[color("159102")]];
            char id_text[id_length] [[color("159102")]];
            int x [[color("5BB24D")]];
            int y [[color("159102")]];
            uint width [[color("5BB24D")]];
            uint height [[color("159102")]];
            bool folded [[color("5BB24D")]];
            uint parent_frame_length [[color("159102")]];
            char parent_frame_text[parent_frame_length] [[color("159102")]];
            uint text_length [[color("5BB24D")]];
            char text_text[text_length] [[color("5BB24D")]];
            u8 color_r [[color("159102")]];
            u8 color_g [[color("159102")]];
            u8 color_b [[color("159102")]];
            u8 color_a [[color("159102")]];
            
            std::core::set_display_name(this, "memo");
        }
        (25): {
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
        (26): {
            uint id_length [[color("000000")]];
            char id_text[id_length] [[color("000000")]];
            uint definition_length [[color("4C4C4C")]];
            char definition_text[definition_length] [[color("4C4C4C")]];
            int x [[color("000000")]];
            int y [[color("4C4C4C")]];
            uint width [[color("000000")]];
            uint height [[color("4C4C4C")]];
            bool folded [[color("000000")]];
            uint parent_frame_length [[color("4C4C4C")]];
            char parent_frame_text[parent_frame_length] [[color("4C4C4C")]];
            
            std::core::set_display_name(this, "subgraph");
        }
        (27): {
            uint id_length [[color("000000")]];
            char id_text[id_length] [[color("000000")]];
            int x [[color("4C4C4C")]];
            int y [[color("000000")]];
            uint width [[color("4C4C4C")]];
            uint height [[color("000000")]];
            bool folded [[color("4C4C4C")]];
            uint parent_frame_length [[color("000000")]];
            char parent_frame_text[parent_frame_length] [[color("000000")]];
            uint index [[color("4C4C4C")]];
            
            std::core::set_display_name(this, "input");
        }
        (28): {
            uint id_length [[color("000000")]];
            char id_text[id_length] [[color("000000")]];
            int x [[color("4C4C4C")]];
            int y [[color("000000")]];
            uint width [[color("4C4C4C")]];
            uint height [[color("000000")]];
            bool folded [[color("4C4C4C")]];
            uint parent_frame_length [[color("000000")]];
            char parent_frame_text[parent_frame_length] [[color("000000")]];
            uint index [[color("4C4C4C")]];
            
            std::core::set_display_name(this, "output");
        }
        (29): {
            uint id_length [[color("3646f5")]];
            char id_text[id_length] [[color("3646f5")]];
            uint from_length [[color("727DF8")]];
            char from_text[from_length] [[color("727DF8")]];
            uint from_port [[color("3646f5")]];
            uint to_length [[color("727DF8")]];
            char to_text[to_length] [[color("727DF8")]];
            uint to_port [[color("3646f5")]];
            uint thickness [[color("727DF8")]];
            u8 color_r [[color("3646f5")]];
            u8 color_g [[color("3646f5")]];
            u8 color_b [[color("3646f5")]];
            u8 color_a [[color("3646f5")]];
            
            uint child_count [[color("2B38C4")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "edge");
        }
        (30): {
            uint id_length [[color("b0b6f5")]];
            char id_text[id_length] [[color("b0b6f5")]];
            int x [[color("C7CBF8")]];
            int y [[color("b0b6f5")]];
            uint size [[color("C7CBF8")]];
            uint parent_frame_length [[color("b0b6f5")]];
            char parent_frame_text[parent_frame_length] [[color("b0b6f5")]];
            u8 color_r [[color("C7CBF8")]];
            u8 color_g [[color("C7CBF8")]];
            u8 color_b [[color("C7CBF8")]];
            u8 color_a [[color("C7CBF8")]];
            
            std::core::set_display_name(this, "joint");
        }
    }
};

// Entry point.
struct Root {
    char magic_header[8] [[color("000000")]];
    uint version_length [[color("111111")]];
    char version[version_length] [[color("111111")]];
    Element element;
};

Root root @ 0x00;
