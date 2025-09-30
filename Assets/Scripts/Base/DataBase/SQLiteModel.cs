using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLiteModel : BaseManager<SQLiteModel>
{
    public Dictionary<long, PlayerIdData> PlayerIdDataClientIdDic = new Dictionary<long, PlayerIdData>();
    public Dictionary<int, PlayerIdData> PlayerIdDataLogicIdDic = new Dictionary<int, PlayerIdData>();
}
