#pragma name agbp
#pragma description ActionGraph Binary Program
#pragma magic [ 00 00 41 47 42 50 00 00 ] @ 0x00

import std.io;

fn count_bits(u8 value) {
    u8 count = 0;
    
    for (u8 i = 0, i < 8, i += 1) {
        if ((value & (1 << i)) != 0)
            count += 1;
    }
    
    return count;
};

fn element_name(u8 id) {
    if (id == 0)
        return "file";
    if (id == 1)
        return "meta";
    if (id == 2)
        return "lang";
    if (id == 3)
        return "idef";
    if (id == 4)
        return "pdef";
    if (id == 5)
        return "ndef";
    if (id == 6)
        return "fdef";
    if (id == 7)
        return "odef";
    if (id == 8)
        return "cdef";
    if (id == 9)
        return "tdef";
    if (id == 10)
        return "ldef";
    if (id == 11)
        return "vdef";
    if (id == 12)
        return "jdef";
    if (id == 13)
        return "node";
    if (id == 14)
        return "joint";
    if (id == 15)
        return "frame";
    if (id == 16)
        return "memo";
    if (id == 17)
        return "edge";
    if (id == 18)
        return "form";
    if (id == 19)
        return "option";
    if (id == 20)
        return "choice";
    if (id == 21)
        return "tuple";
    if (id == 22)
        return "list";
    if (id == 23)
        return "arg";
    if (id == 24)
        return "loc";
    if (id == 25)
        return "out";
};

fn has_children(u8 id) {
    if (id == 1 || id == 2 || id == 4 ||
        id == 11 || id == 12 || id == 14 ||
        id == 15 || id == 16 || id == 17 ||
        id == 24 || id == 25)
        return false;

    return true;
};

struct Text {
    u8 length [[color("FF0000")]];
    char string[length] [[color("FF8888")]];
};

struct Version {
    u8 length [[color("888888")]];
    char string[length] [[color("FFFFFF")]];
};


struct Element {
    u8 id [[color("00FFFF")]];
    u8 attr_mask [[color("0000FF")]];
    Text attr_values[count_bits(attr_mask)];
    if (has_children(id)) {
        u8 child_count [[color("FFFF00")]];
        Element children[child_count];
    }
}[[name(element_name(id))]];

struct Root {
    u64 magic [[color("000000")]];
    Version version;
    Element root;
};

Root root @ 0x00;

std::print("0x{:08X}", root.magic);
