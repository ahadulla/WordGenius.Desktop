using System.Collections.Generic;

namespace WordGenius.Desktop.Entities.Translator;

public class TranslationResponse
{
    public DataObject data { get; set; }
}

public class DataObject
{
    public List<TranslationObject> translations { get; set; }
}

public class TranslationObject
{
    public string translatedText { get; set; }
}
