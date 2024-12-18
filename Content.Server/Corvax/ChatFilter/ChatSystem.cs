using System.Linq;
using System.Text.RegularExpressions;

namespace Content.Server.Chat.Systems;

public sealed partial class ChatSystem
{
    private static readonly Dictionary<string, string> SlangReplace = new()
    {
        { "го", "давай" },
        { "геник", "генератор" },
        { "кк", "красный код" },
        { "ск", "синий код" },
        { "зк", "зелёный код" },
        { "пда", "кпк" },
        { "корп", "корпоративный" },
        { "мш", "имплант защиты разума" },
        { "трейтор", "предатель" },
        { "в инж", "в инженерный" },
        { "инжи", "инженеры" },
        { "инжы", "инженеры" },
        { "инжу", "инженеру" },
        { "инжам", "инженерам" },
        { "инжинер", "инженер" },
        { "яо", "ядерные оперативники" },
        { "нюк", "ядерный оперативник" },
        { "нюкеры", "ядерные оперативники" },
        { "нюкер", "ядерный оперативник" },
        { "нюкеровец", "ядерный оперативник" },
        { "нюкеров", "ядерных оперативников" },
        { "аирлок", "шлюз" },
        { "аирлоки", "шлюзы" },
        { "айрлок", "шлюз" },
        { "айрлоки", "шлюзы" },
        { "визард", "волшебник" },
        { "дизарм", "толчок" },
        { "синга", "сингулярность" },
        { "сингу", "сингулярность" },
        { "синги", "сингулярности" },
        { "сингой", "сингулярностью" },
        { "разгерм", "разгерметизация" },
        { "разгермой", "разгерметизацией" },
        { "арт", "артефакт" },
        { "арты", "артефакты" },
        { "анома", "аномалия" },
        { "аномы", "аномалии" },
        { "норм", "нормально" },
        { "хз", "не знаю" },
        { "синд", "синдикат" },
        { "пон", "понятно" },
        { "непон", "не понятно" },
        { "нипон", "не понятно" },
        { "кста", "кстати" },
        { "кст", "кстати" },
        { "плз", "пожалуйста" },
        { "пж", "пожалуйста" },
        { "спс", "спасибо" },
        { "сяб", "спасибо" },
        { "прив", "привет" },
        { "привед", "привет" },
        { "превед", "привет" },
        { "ок", "окей" },
        { "чел", "мужик" },
        { "лан", "ладно" },
        { "збс", "заебись" },
        { "мб", "может быть" },
        { "оч", "очень" },
        { "омг", "боже мой" },
        { "нзч", "не за что" },
        { "пок", "пока" },
        { "бб", "пока" },
        { "пох", "плевать" },
        { "ясн", "ясно" },
        { "всм", "всмысле" },
        { "чзх", "что за херня?" },
        { "изи", "легко" },
        { "гг", "хорошо сработано" },
        { "пруф", "доказательство" },
        { "пруфы", "доказательства" },
        { "пруфани", "докажи" },
        { "пруфанул", "доказал" },
        { "имба", "нечестно" },
        { "имбулечка", "нечестно" },
        { "разлокать", "разблокировать" },
        { "юзать", "использовать" },
        { "юзай", "используй" },
        { "юзнул", "использовал" },
        { "хилл", "лечение" },
        { "хильни", "полечи" },
        { "подхиль", "полечи" },
        { "хелп", "помоги" },
        { "хелпани", "помоги" },
        { "хелпанул", "помог" },
        { "рофл", "прикол" },
        { "рофлишь", "шутишь" },
        { "крч", "короче говоря" },
        { "шатл", "шаттл" },
        { "т.д", "так далее" },
        { "тд", "так далее" },
        { "пр", "привет" },
        { "плашь", "плащ" },
        { "плащь", "плащ" },
        { "меда", "медицинского отсека" },
        { "меду", "медицинскому отсеку" },
        { "ио", "инженерный отдел" },
        { "каргонец", "снабженец" },
        { "каргонцы", "снабженцы" },
        // Reagents
        { "бикардин", "бикаридин" },
        { "бика", "бикаридин" },
        { "бику", "бикаридин" },
        { "декса", "дексалин" },
        { "дексу", "дексалин" },
        { "дирмалин", "дермалин" },
        { "дерма", "дермалин" },
        { "дерму", "дермалин" },
        { "дило", "диловен" },
        { "эпиф", "эпинефрин" },
        // Jobs SS13
        { "хоп", "гп" },
        { "хос", "гсб" },
        { "хоса", "гсб" },
        { "смо", "главрач" },
        { "се", "си" },
        { "рд", "нр" },
        // Jobs SS14
        { "кеп", "капитан" },
        { "кепа", "капитану" },
        { "кепу", "капитану" },
        { "кэпа", "капитана" },
        { "кэпу", "капитану" },
        { "кэпом", "капитаном" },
        { "гпе", "главе персонала" },               // wtf
        { "гсбе", "главе службы безопасности" },    // wtf
        { "гсбу", "главу службы безопасности" },    // wtf
        { "гву", "главврачу" },                     // wtf
        { "нр", "научный руководитель" },
        { "нра", "научного руководителя" },
        { "нру", "научному руководителю" },
        { "нром", "научным руководителем" },
        { "км", "квартирмейстер" },
        { "кма", "квартирмейстера" },
        { "кму", "квартирмейстеру" },
        { "кмом", "квартирмейстером" },
        { "вард", "смотритель" },
        { "варден", "смотритель" },
        { "вардена", "смотрителя" },
        { "дэк", "детектив" },
        { "дэку", "детективу" },
        { "дэка", "детектива" },
        { "дек", "детектив" },
        { "деку", "детективу" },
        { "дека", "детектива" },
        { "бм", "бригмед" },
        { "бма", "бригмеда" },
        { "бму", "бригмеду" },
        { "бмом", "бригмедом" },
        { "парамед", "парамедик" },
        { "админ", "администратор" },
        { "админы", "администраторы" },
        { "админов", "администраторов" },
        // OOC
        { "афк", "ссд" },
        { "забанят", "покарают" },
        { "бан", "наказание" },
        { "пермач", "наказание" },
        { "перм", "наказание" },
        { "запермили", "наказание" },
        { "запермят", "накажут" },
        { "нонрп", "плохо" },
        { "нрп", "плохо" },
        { "ерп", "ужас" },
        { "рдм", "плохо" },
        { "дм", "плохо" },
        { "гриф", "плохо" },
        { "фрикил", "плохо" },
        { "фрикилл", "плохо" },
        { "лкм", "левая рука" },
        { "пкм", "правая рука" },
    };

    private static readonly Dictionary<string, string> SoloSlangReplace = new()
    {
        { "сщ", "синий щит" },
        { "осщ", "офицер синий щит" },
        { "гп", "глава персонала" },
        { "си", "старший инженер" },
        { "гв", "главрач" },
        { "доде", "добрый день" },                  // wtf
        { "удсм", "удачной смены" },                // wtf
        { "?", "м?" },
    };

    private string ReplaceWords(string message)
    {
        if (string.IsNullOrEmpty(message))
            return message;

        message = Regex.Replace(message, "^(\\w+)$", match =>
        {
            bool isUpperCase = match.Value.All(Char.IsUpper);

            if (SoloSlangReplace.TryGetValue(match.Value.ToLower(), out var replacement))
                return isUpperCase ? replacement.ToUpper() : replacement;
            return match.Value;
        });

        return Regex.Replace(message, "\\b(\\w+)\\b", match =>
        {
            bool isUpperCase = match.Value.All(Char.IsUpper);

            if (SlangReplace.TryGetValue(match.Value.ToLower(), out var replacement))
                return isUpperCase ? replacement.ToUpper() : replacement;
            return match.Value;
        });
    }
}
