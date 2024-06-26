﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TactileModules.UIElementsCodeBehind {

    [DebuggerDisplay("{Type} (name='{Name}')")]
    internal class UxmlNode
    {
        /// <summary>
        /// Gets the current UXML node type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the UXML node name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets a boolean value that indicates if the current UXML node is the root node.
        /// </summary>
        public bool IsRoot { get; }
    
        /// <summary>
        /// Should code behind generation take place for this UXML document.
        /// </summary>
        public bool GenerateCodeBehind { get; set; }
    
        /// <summary>
        /// What namespace should the code behind be generated under.
        /// </summary>
        public string Namespace { get; set; }
        
        /// <summary>
        /// The name of the event to generate.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// The path to load the UXML document from, and auto initialize
        /// </summary>
        public string ResourcePath { get; set; }

        /// <summary>
        /// The path to load the USS documents from, and auto apply. Can be a comma separated list.
        /// </summary>
        public string StylesheetsPath { get; set; }
        
        /// <summary>
        /// Gets the UXML child nodes.
        /// </summary>
        public IList<UxmlNode> Children { get; } = new List<UxmlNode>();

        /// <summary>
        /// Gets a boolean value that indicates if the current UXML node has a name.
        /// </summary>
        public bool HasName => !string.IsNullOrEmpty(Name);


        /// <summary>
        /// Creates a new <see cref="UxmlNode"/> instance.
        /// </summary>
        /// <param name="type">Node type.</param>
        /// <param name="name">Node name.</param>
        /// <param name="isRoot">Is root node.</param>
        public UxmlNode(Type type, string name, bool isRoot = false)
        {
            Type = type;
            Name = name;
            IsRoot = isRoot;
        }
    }

}