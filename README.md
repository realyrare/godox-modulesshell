#  GodOxModulesShell
<div align="center">

    如果对您有帮助，点击右上角⭐Star⭐关注 ，感谢支持开源！
</div>


#### 介绍

 **基于dotNetCore基础之上开发的模块化框架。核心思想借鉴了ABP VNext,支持依赖注入，轻量级，适用于中小项目模块化。业务模块CRUD API接口抽象到上层controller,只需要建立对应的实体，即可生成某张表的增删改查接口，未来将往低代码的方向发展** 
 
 所有的代码坚持" **Don't repeat yourself** "。坚决反对" **简单的事情复杂干** "，" **大量代码在应用层过度层层封装** "，反对" **过度设计** "的原则下进行开发。

 如果你有这个习惯，并且也想找一款迷你型、上手快，不用学习太多东西的框架，那么它就适合你，只要你会dotnet core,几乎没有学习成本。

#### 单机架构

 **该项目还有个单体架构地址：[适合小白入门进阶，更适用(实用)老手上路干活的dotnetcore框架](https://gitee.com/shenniu_code_group/shen-nius.-modularity)** 

#### 特别申明

 注意：**该项目新功能的迭代今后我会放在模块化分支上面，单体新功能的迭代一般可能会在最新的asp.netcore版本上开发。老版本新功能有偿服务！** 

#### 软件架构

主要是使用已经千锤百炼的组件进行积木组装，然后结合业务快速开发，配合顺手的ORM，不管开发API还是mvc效率和性能都是杠杠的。


#### 使用技术

1. API框架：dotNetCore 
2. ORM：SqlSugarCore 
3. 缓存：MemoryCache和redis一键任意切换
4. 日志管理：Nlog
5. 工具类：Aes加密、Md5加密、RSA加密、Des加密  
6. token:Json Web Token
7. 实体验证：FluentValidation
8. 实体映射：AutoMapper
9. 数据库使用mysql（后面会做sqlserver等其他数据库的兼容）
10. 支持跨平台部署 linux/windows
11. 支持AOP缓存，使用AspectCore,缓存可做到Memarycache和redis一键切换
12. 支持AOP事务，服务层和控制器都可以打上特性标签使用
13. 对多租户使用Filter，不管是添加还是更新、查询即可自动赋值
14. 支持七牛云和本地图片一键切换使用上传
15. 对于单表的增删改查，在控制器内做了封装，有新的业务按约定建立对应的CRUD实体，一套API自动完成


#### 安装教程

### 标题

1.  git clone -b master https://gitee.com/shenniu_code_group/godox-modulesshell 
2.  在mysql上创建数据库，然后执行源码doc文件夹下的sql脚本语句，以最近日期为准。doc文件夹里面数据库字典文档，word、CHM、html格式的都有。
3. GodOx.Mvc.Admin（前后端不分离）和GodOx.API.Hosting（前后端分离 API）可以配置你要启动的appsettings.json文件信息。
4、 使用过程中有什么问题欢迎提issues,基本都会第一时间解决。
#### 使用说明

1.   ShenNius.Share.Infrastructure 基础设施、里面包含常用的扩展方法、静态类。
2.   ShenNius.Share.Models 实体层、里面包括Dto验证、配置类。

### 标题
3.   ShenNius.Share.Domain 服务层、业务逻辑基本都在这个里面、里面包含了数据访问操作。
4.   ShenNius.Admin.API 有对服务层CRUD的抽象处理，模块只需按规则建立对应的CRUD实体，API接口自动生成。
5.   ShenNius.API.Hosting 前后端分离的API。
6.   ShenNius.Mvc.Admin 基于layui的后台管理包含CMS、商城，权限管理。


#### 案例效果图

  #### 权限管理模块

<table>
    <tr>
        <td><img src="https://images.gitee.com/uploads/images/2021/1001/200307_6fa1bb44_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0925/235614_89800b14_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0304/164851_824fb005_1173871.png"/></td>
    </tr>
    <tr>
        <td><img src="https://images.gitee.com/uploads/images/2021/0925/235614_89800b14_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0925/235614_89800b14_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0925/235838_37691e67_1173871.png"/></td>
    </tr>              
   <tr>
      <td><img src="https://images.gitee.com/uploads/images/2021/0925/235909_4c7185de_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220143_65141036_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220202_2220b39c_1173871.png"/></td>
    </tr> 
   <tr>
     <td><img src="https://images.gitee.com/uploads/images/2021/0927/220246_a7f94f1e_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220307_c9bedea9_1173871.png"/></td>
   <td><img src="https://images.gitee.com/uploads/images/2021/0927/220220_98eeadcf_1173871.png"/></td>       
    </tr> 
</table>

  #### CMS系统模块
<table>                
   <tr>
      <td><img src="https://images.gitee.com/uploads/images/2021/0927/220844_6f81e7f8_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220609_911f705d_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220630_db845562_1173871.png"/></td>
    </tr> 
   <tr>
     <td><img src="https://images.gitee.com/uploads/images/2021/0927/220643_c0c4885d_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220704_d03f4bc4_1173871.png"/></td>
       <td><img src="https://images.gitee.com/uploads/images/2021/0927/220323_f940ac46_1173871.png"/></td>
    </tr>
</table>

  #### 商城系统模块
<table>                
   <tr>
      <td><img src="https://images.gitee.com/uploads/images/2021/0927/220934_03c65880_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/220950_1b6df0a5_1173871.png"/></td>
        <td><img src="https://images.gitee.com/uploads/images/2021/0927/221005_423e3c97_1173871.png"/></td>
    </tr> 
   <tr>
     <td><img src="https://images.gitee.com/uploads/images/2021/0927/221028_39281d75_1173871.png"/></td>
    </tr>
</table>

#### 入群交流
QQ群：<a target="_blank" href="https://qm.qq.com/cgi-bin/qm/qr?k=IlNhUh4OZ4IS0fjt2O6b8HtjKuxiNY3I&jump_from=webapi"><img border="0" src="//pub.idqqimg.com/wpa/images/group.png" alt="dotnet根据地" title="dotnet根据地">878303823</a>

微信群：喜欢微信交流的扫描下面我的个人二维码，邀请进群。

#### 项目赞助及微信联系

<table>                
   <tr>
      <td><img src="doc/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20220125182738.jpg"/></td>
        <td><img src="doc/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20220125182747.jpg"/></td>
 <td><img src="doc/%E5%BE%AE%E4%BF%A1%E5%9B%BE%E7%89%87_20220125182730.jpg"/></td>     
    </tr>
</table>

#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request

