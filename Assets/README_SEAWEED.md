
#入口脚本
入口脚本 Main > 加载预制体 Load (挂mono脚本Load)


# 配置文件
脚本: StartUpConfig 

* 服务器地址
* 热更包地址（项目地址 或 本地地址）

# 热更包：

* UnityWebSocket.Runtime
* Base 
* Custom

加载顺序：  UnityWebSocket.Runtime > Base  > Custom

## 脚本路径

Assets/Scripts/Custom

# 打包
打包后的资源路劲： Assets/StreamingAssets/AssetBundles
打包后的代码路劲（包含AOT）： Assets/StreamingAssets/Lib

打包资源： Assets/Editor/BuildAssetBundle.cs

    [MenuItem("Tools/Build AssetBundles")]
	
打包脚本： Assets/Editor/CopyDll.cs

    [MenuItem("Tools/Copy Dll")]
	
	
预制体存放的文件夹: Assets/Prefabs




# Flutter风格的UI界面

Flutter风格的UI组件路径： Assets/Prefabs/IO
AB包名： "io"


脚本路劲： Assets/Scripts/Base/IO

美术资源路径： Assets/Sprites/UI/IO












# 页面类


## View： 主类


# 后台页面

## IOCanvasView： 后台页面类

* 资源加载

* IOCanvasModel.Instance.state ？？



## IOCanvasManager： 这个类是和算法开交互的类


## IOPasswordPanel:   这个是键盘面板？？

* 是页面[IOCanvasView]的子节点。
* 用于键盘输入








# 游戏彩金接口：

JackpotBetHost



脚本：NetCmd

机台给彩金后台发： C2S_JackBet






这个脚本用类实现服务器
* NetMessageController 【脚本】后台服务器接受到前端机台的协议




* 【问题】ClientWS  这客户端脚本被调用！！ 要解决


NetMgr/ClientWS 这脚本被调用！！！！！





# 脚本 Controller：  走20019协议给彩金后台传入 major 和 grand

    public void OnJackpotBet(SBoxJackpotBet sBoxJackpotBet)
    {
        //Dictionary<string, string> postDic = new Dictionary<string, string>
        //{
        //    { "record", JsonConvert.SerializeObject(sBoxJackpotBet) }
        //};
        //Utils.Post("http://192.168.3.152/api/bonus_bet", postDic);
        SQLite.Instance.sBoxJackpotBets.Enqueue(sBoxJackpotBet);

        SBoxIdea.JackpotBetHost(sBoxJackpotBet);
    }







# UI框架

## IOCanvasView 
* 一个根页面节点，不同的子页面通过删除根节点下的内容，重新创建新内容来实现。
* 不同子页面，通过状态 IOState 来标记。

* 【？】 根节点页面叫View,子页面叫Panel
* IOCanvasView 是后台界面的主页




## IOCanvasManager

* 统一请求后台数据，并结包到model中，再事件通知到View
*



InstantiateBaseShow() 显示文字


InstantiateBlank();
InstantiateBlank();



## 返回按钮的写法：

GameObject parent = new GameObject();
tempObjList.Add(parent);
parent.AddComponent<HorizontalLayoutGroup>();
parent.transform.parent = menuPanel;
parent.transform.localScale = Vector3.one;
InstantiateBaseBtn("Return", ReturnToFunction, parent: parent.transform, style: 2, scale: 1f);
var baseShow2 = InstantiateEasyShow(Utils.GetLanguage("ClickCancleTips"), "", parent: transform, fontSize: 40);



private IOBaseBtn InstantiateBaseBtn(string str, 
UnityAction unityAction, 
bool showBg = true, 
TextAnchor textAnchor = TextAnchor.MiddleCenter, 
Transform parent = null, 
int style = 1, 
int fontSize = 20, 
float scale = 1)










# ===================================================================================================


# 页面预制体：

ui： 主界面

IOCanvas： 后台主页面



EditIdDeleteBtnClick()  ：按键


# IOCanvasView： 后台页面


IOCanvasView： 后台页面脚本



IOBaseBtn InstantiateBaseBtn(...) // 后台主从菜单页-添加子项按钮
 
 
void InitFunctionPanel() // 菜单页面入口


InitParamsPanel() // 参数设置界面



IOSectionState.GroupId

selectSection.selected


IOCanvasModel.Instance.tempCfgData.
 
 
## 参数设置的加减回调脚本： 
 
IOCanvasViewController :  后台修改参数，加减回调脚本。
 
void KeyUp(bool isAdd) ： 加减回调函数。



## 长按箭头加、长按箭头减（快速加减）

SelectedSectionDownAndLeft ： 长按减函数

SelectedSectionUpAndRight ： 长按加函数


## 确定按钮，返回按钮 -- 回调：

void OnRefreshIOCanvas() ： 保存桉树修改

 -- IOState State
 
### IOState ： 【枚举】 ： 后台那个界面：
  State = IOState.Params; -- 参数设置界面

 
         Debug.LogError("OnRefreshIOCanvas"); ？？
		 
		  Confirm ？？
		  
		  
		  
		  
		  IOCanvasModel.Instance.groupId}  tempGroupId: {IOCanvasModel.Instance.tempGroupId
		  
		  
		  
# 其他		  





# 弹窗接口

提示弹窗 ： IOPopTips.Instance.ShowTips(Utils.GetLanguage("Comming soon"))

"Comming soon":"敬请期待",




//#seaweed# 

/*#seaweed#  */


# 参照做法
参照IOSectionState.CountDown:的做法



# 远程连接硬件调试：

SBoxInit.Instance.Init("192.168.3.187", OnInitSBox);
	
	
	
# =========Modle

#  IOCanvasModel

* 后台彩金数据存储的model
	
	
	
# =========网络层：


## NetMgr

* NetMgr 可以选择 ServerWS 或 ClientWS

* 服务器给客户端，或客户端给服务器 发数据的 统一接口。


Messenger.AddListener<WSSrvMsgData>(MessageName.Event_NetworkWSServerData, OnWSServerData); // 客户端发给服务器的数据
Messenger.AddListener<byte[]>(MessageName.Event_NetworkClientData, OnClientData);  // 服务器发给客户端的数据
	
	
## ServerWS

* udp 广播？

*  StartServer 创建udp 

-- 开启udp 

-- 开启websocket

-- 接收到 客户端 udp的请求时， 返回服务器的"ip"和"端口"







## ClientWS

* udp 广播 ？

* websocket 给服务器发数据。

* StartUdp() : 
-- 开启udp 
-- 监听udp数据
-- 定时发送udp数据 请求服务器ip和端口
拿到服务器地址后，停止定时发udp,开始websocket的连接

	
	

	
	
## NetMessageController

* 彩金后台-响应分机彩金押注的业务逻辑
	
	
	
	
	
	
	NetCmd
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	

