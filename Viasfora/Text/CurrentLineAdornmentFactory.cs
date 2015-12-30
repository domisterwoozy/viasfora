﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace Winterdom.Viasfora.Text {

  [Export(typeof(IWpfTextViewCreationListener))]
  [ContentType(ContentTypes.Text)]
  [TextViewRole(PredefinedTextViewRoles.Interactive)]
  internal sealed class CurrentLineAdornmentFactory : IWpfTextViewCreationListener {
    [Import]
    public IClassificationTypeRegistryService ClassificationRegistry = null;
    [Import]
    public IClassificationFormatMapService FormatMapService = null;
    [Import]
    public IVsfSettings Settings = null;

    [Export(typeof(AdornmentLayerDefinition))]
    [Name(Constants.LINE_HIGHLIGHT)]
    [Order(Before = PredefinedAdornmentLayers.Selection)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public AdornmentLayerDefinition editorAdornmentLayer = null;

    public void TextViewCreated(IWpfTextView textView) {
      IClassificationType classification =
         ClassificationRegistry.GetClassificationType(Constants.LINE_HIGHLIGHT);
      IClassificationFormatMap map =
         FormatMapService.GetClassificationFormatMap(textView);
      textView.Properties.GetOrCreateSingletonProperty(
        () => {
          return new CurrentLineAdornment(textView, map, classification, Settings);
        });
    }
  }
}
