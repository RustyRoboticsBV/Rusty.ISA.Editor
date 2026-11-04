////////////////////////////////////////////////////////////////////////////////
// WARNING:                                                                   //
// This is a generated script file. It should not be edited directly.         //
// See the Generators/ directory to view the source generation module.        //
////////////////////////////////////////////////////////////////////////////////

namespace Rusty.ActionGraph.Serialization;

/// <summary>
/// An element that represents a group of child elements of some type.
/// </summary>
internal interface IElementGroup<ChildElementT>
    where ChildElementT : Element
{
    /// <summary>
    /// Get a child element of the return type with a specific ID. Returns null if the child element cannot be found.
    /// </summary>
    public ChildElementT Find(string id)
    {
        Element _this = this as Element;
        for (int i = 0; i < _this.ChildCount; i++)
        {
            if (_this.GetChild(i) is ChildElementT child && child.ID == id)
                return child;
        }
        return null;
    }
}