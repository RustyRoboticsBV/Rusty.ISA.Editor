////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

using System;
using System.Security.Cryptography;

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// A utility for computing the hashcode of a element.
/// </summary>
internal static class Hasher
{
    /* Public methods. */
    /// <summary>
    /// Compute the hashcode of a file and its children, and store it in the element.
    /// </summary>
    public static void StoreHash(FileElement file, HashAlgorithm hash)
    {
        // Compute checksum.
        string hashHex = Hash(file, hash);

        // Store the checksum.
        file.DefaultLanguage = hashHex;
    }

    /// <summary>
    /// Compute the hashcode of a element and its children.
    /// </summary>
    public static string Hash(Element element, HashAlgorithm hash)
    {
        Hash(hash, element, [0x00, 0x00, 0x00, 0x00]);
        hash.TransformFinalBlock([], 0, 0);
        return Convert.ToHexString(hash.Hash);
    }

    /* Private methods. */
    /// <summary>
    /// Compute the hashcode of this element and its children.
    /// </summary>
    private static void Hash(HashAlgorithm hash, Element element, byte[] buffer)
    {
        // Hash tag.
        Hash(hash, element.ElementName.Length, buffer);
        Hash(hash, element.ElementName, buffer);
        
        // Hash attributes.
        int count = element.AttributeCount;
        if (element is FileElement)
            count--;
        
        for (int i = 0; i < element.AttributeCount; i++)
        {
            string attributeName = element.NameOfAttribute(i);
            
            if (attributeName == "default_language")
                continue;
            
            Hash(hash, attributeName.Length, buffer);
            Hash(hash, attributeName, buffer);
            
            string valueStr = element.GetAttribute(i).ToString();
            Hash(hash, valueStr.Length, buffer);
            Hash(hash, valueStr, buffer);
        }
        
        // Hash child nodes.
        Hash(hash, element.ChildCount, buffer);
        for (int i = 0; i < element.ChildCount; i++)
        {
            Hash(hash, element.GetChild(i), buffer);
        }
    }
    
    /// <summary>
    /// Compute the hashcode of a string.
    /// </summary>
    private static void Hash(HashAlgorithm hash, string str, byte[] buffer)
    {
        for (int i = 0; i < str.Length; i++)
        {
            Hash(hash, str[i], buffer);
        }
    }
    
    /// <summary>
    /// Compute the hashcode of a character.
    /// </summary>
    private static void Hash(HashAlgorithm hash, char chr, byte[] buffer)
    {
        buffer[0] = (byte)chr;
        buffer[1] = (byte)(chr >> 8);
        hash.TransformBlock(buffer, 0, 2, null, 0);
    }

    /// <summary>
    /// Compute the hashcode of an integer.
    /// </summary>
    private static void Hash(HashAlgorithm hash, int value, byte[] buffer)
    {
        buffer[0] = (byte)value;
        buffer[1] = (byte)(value >> 8);
        buffer[2] = (byte)(value >> 16);
        buffer[3] = (byte)(value >> 24);
        hash.TransformBlock(buffer, 0, 4, null, 0);
    }
}