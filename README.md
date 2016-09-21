# ExLog
为什么会写这个,因为平时写网站,写软件,异常总是有的,但是每次存放日志方式不是.txt,就是数据库,感觉都不太好,因为.txt太难查看了,数据库又太笨重,所以就出现了ExLog,ExLog是一个基于Xml,用于记录异常信息以及调试信息的日志集,这个类是用C#写的。

## 如何使用
引入
```
using ExLog;
```
如果你是web程序,使用**WebLog**即可。
```
try{
    
}
catch(Exception ex){
    Ex.WebLog.Write("This is WebError",ex);
}
```

如果你是应用程序/服务,使用**AppLog**即可。
```
try{
    
}
catch(Exception ex){
    Ex.AppLog.Write("This is AppError",ex);
}
```
## 如何读取
获取所有异常所发生的日期
```
Ex.WebLog.Read_ExAllPath();
```
获取指定日期的异常集合
```
Ex.WebLog.Read_ExListByDay(DateTime.Now);
```
获取当日异常集合
```
Ex.WebLog.Read_ExListByDay();
```
读取异常详细信息
```
List<Log_Obj> Lst_Log=Ex.WebLog.Read_ExDetial(DateTime.Now,"TypeName");
```

