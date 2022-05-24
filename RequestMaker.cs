using ItemChanger;
using RandomizerMod.RandomizerData;
using RandomizerMod.RC;

namespace RandomizeGladeAccess
{
    public static class RequestMaker
    {
        public static void Hook()
        {
            RequestBuilder.OnUpdate.Subscribe(-499, SetupRefs);
            RequestBuilder.OnUpdate.Subscribe(50f, AddGladeAccess); 
        }

        private static void SetupRefs(RequestBuilder rb)
        {
            rb.EditItemRequest(Consts.GladeAccessItemName, info =>
            {
                info.getItemDef = () => new ItemDef()
                {
                    Name = Consts.GladeAccessItemName,
                    Pool = Consts.GladeAccessPoolGroup,
                    MajorItem = false,
                    PriceCap = 500,
                };
            });
            
            rb.OnGetGroupFor.Subscribe(0f, MatchGladeAccessGroup);

            static bool MatchGladeAccessGroup(RequestBuilder rb, string item, RequestBuilder.ElementType type, out GroupBuilder gb)
            {
                if (item == Consts.GladeAccessItemName && type is RequestBuilder.ElementType.Unknown or RequestBuilder.ElementType.Item)
                {
                    gb = rb.GetGroupFor(ItemNames.Elegant_Key);
                    return true;
                }
                gb = default;
                return false;
            }
        }

        private static void AddGladeAccess(RequestBuilder rb)
        {
            if (RandomizeGladeAccess.GS.Enabled)
                rb.AddItemByName(Consts.GladeAccessItemName, RandomizeGladeAccess.GS.AddDupe ? 2 : 1);
        }
    }
}