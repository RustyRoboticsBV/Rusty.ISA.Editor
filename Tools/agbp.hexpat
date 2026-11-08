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
            string id [[color("CC6666")]];
            string editor [[color("CC6666")]];
            string schema_version [[color("CC6666")]];
            string format_version [[color("CC6666")]];
            string default_language [[color("CC6666")]];
            string checksum [[color("CC6666")]];
            
            uint child_count [[color("CC6666")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "file");
        }
        (1): {
            string id [[color("CC7E66")]];
            
            std::core::set_display_name(this, "language_definition");
        }
        (2): {
            string id [[color("CC9566")]];
            string value [[color("CC9566")]];
            
            uint child_count [[color("CC9566")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "metadata");
        }
        (3): {
            string id [[color("CCAD66")]];
            string execution_handler [[color("CCAD66")]];
            
            uint child_count [[color("CCAD66")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "instruction_definition");
        }
        (4): {
            string id [[color("CCC466")]];
            bool localizable [[color("CCC466")]];
            
            std::core::set_display_name(this, "parameter_definition");
        }
        (5): {
            string id [[color("BCCC66")]];
            
            uint child_count [[color("BCCC66")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "node_definition");
        }
        (6): {
            string id [[color("A5CC66")]];
            string instruction [[color("A5CC66")]];
            
            uint child_count [[color("A5CC66")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "form_definition");
        }
        (7): {
            string id [[color("8DCC66")]];
            
            uint child_count [[color("8DCC66")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "option_definition");
        }
        (8): {
            string id [[color("76CC66")]];
            
            uint child_count [[color("76CC66")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "choice_definition");
        }
        (9): {
            string id [[color("66CC6E")]];
            
            uint child_count [[color("66CC6E")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "tuple_definition");
        }
        (10): {
            string id [[color("66CC85")]];
            
            uint child_count [[color("66CC85")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "list_definition");
        }
        (11): {
            string id [[color("66CC9D")]];
            string parameter [[color("66CC9D")]];
            
            std::core::set_display_name(this, "field_definition");
        }
        (12): {
            string id [[color("66CCB4")]];
            string parameter [[color("66CCB4")]];
            bool remove_default_port [[color("66CCB4")]];
            
            std::core::set_display_name(this, "port_definition");
        }
        (13): {
            string id [[color("66CCCC")]];
            string definition [[color("66CCCC")]];
            int x [[color("66CCCC")]];
            int y [[color("66CCCC")]];
            uint width [[color("66CCCC")]];
            uint height [[color("66CCCC")]];
            bool folded [[color("66CCCC")]];
            string parent_frame [[color("66CCCC")]];
            string text [[color("66CCCC")]];
            color color [[color("66CCCC")]];
            string start_point_name [[color("66CCCC")]];
            
            uint child_count [[color("66CCCC")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "node");
        }
        (14): {
            string id [[color("66B4CC")]];
            int x [[color("66B4CC")]];
            int y [[color("66B4CC")]];
            uint width [[color("66B4CC")]];
            uint height [[color("66B4CC")]];
            bool folded [[color("66B4CC")]];
            string parent_frame [[color("66B4CC")]];
            string text [[color("66B4CC")]];
            color color [[color("66B4CC")]];
            
            std::core::set_display_name(this, "memo");
        }
        (15): {
            string id [[color("669DCC")]];
            int x [[color("669DCC")]];
            int y [[color("669DCC")]];
            uint width [[color("669DCC")]];
            uint height [[color("669DCC")]];
            bool folded [[color("669DCC")]];
            string parent_frame [[color("669DCC")]];
            string text [[color("669DCC")]];
            color color [[color("669DCC")]];
            
            std::core::set_display_name(this, "frame");
        }
        (16): {
            string id [[color("6685CC")]];
            string edge [[color("6685CC")]];
            int x [[color("6685CC")]];
            int y [[color("6685CC")]];
            uint size [[color("6685CC")]];
            string parent_frame [[color("6685CC")]];
            string text [[color("6685CC")]];
            color color [[color("6685CC")]];
            
            std::core::set_display_name(this, "joint");
        }
        (17): {
            string id [[color("666ECC")]];
            string _from [[color("666ECC")]];
            uint port [[color("666ECC")]];
            string to [[color("666ECC")]];
            uint thickness [[color("666ECC")]];
            string text [[color("666ECC")]];
            color color [[color("666ECC")]];
            
            std::core::set_display_name(this, "edge");
        }
        (18): {
            string id [[color("7666CC")]];
            string definition [[color("7666CC")]];
            bool folded [[color("7666CC")]];
            string text [[color("7666CC")]];
            color color [[color("7666CC")]];
            
            uint child_count [[color("7666CC")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "form");
        }
        (19): {
            string id [[color("8D66CC")]];
            string definition [[color("8D66CC")]];
            bool enabled [[color("8D66CC")]];
            bool folded [[color("8D66CC")]];
            string text [[color("8D66CC")]];
            color color [[color("8D66CC")]];
            
            uint child_count [[color("8D66CC")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "option");
        }
        (20): {
            string id [[color("A566CC")]];
            string definition [[color("A566CC")]];
            string selected [[color("A566CC")]];
            bool folded [[color("A566CC")]];
            string text [[color("A566CC")]];
            color color [[color("A566CC")]];
            
            uint child_count [[color("A566CC")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "choice");
        }
        (21): {
            string id [[color("BC66CC")]];
            string definition [[color("BC66CC")]];
            bool folded [[color("BC66CC")]];
            string text [[color("BC66CC")]];
            color color [[color("BC66CC")]];
            
            uint child_count [[color("BC66CC")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "tuple");
        }
        (22): {
            string id [[color("CC66C4")]];
            string definition [[color("CC66C4")]];
            bool folded [[color("CC66C4")]];
            string text [[color("CC66C4")]];
            color color [[color("CC66C4")]];
            
            uint child_count [[color("CC66C4")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "list");
        }
        (23): {
            string id [[color("CC66AD")]];
            string definition [[color("CC66AD")]];
            string value [[color("CC66AD")]];
            string text [[color("CC66AD")]];
            color color [[color("CC66AD")]];
            
            uint child_count [[color("CC66AD")]];
            Element children[child_count];
            
            std::core::set_display_name(this, "field");
        }
        (24): {
            string id [[color("CC6695")]];
            string definition [[color("CC6695")]];
            uint index [[color("CC6695")]];
            string text [[color("CC6695")]];
            color color [[color("CC6695")]];
            
            std::core::set_display_name(this, "port");
        }
        (25): {
            string id [[color("CC667E")]];
            string language [[color("CC667E")]];
            string value [[color("CC667E")]];
            
            std::core::set_display_name(this, "localized_value");
        }
    }
};

// Entry point.
struct Root {
    u8 magic_header[8];
    string version;
    Element element;
};

Root root @ 0x00;
