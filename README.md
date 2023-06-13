# AresInspector

[![license](http://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/YannABC/AresInspector/main/LICENSE)

AresInspector 是 类似odin的Unity编辑器脚本，使用Attribute的方式来制作编辑器。
和odin不同的是，AresInspector更加轻量，使用了纯UIToolkit(VisualEment),没有使用IMGUI。

包含Layout,Drawer,Config三大类。

![examples](./docs/images/examples.png)
![examples](./docs/images/example-layout.png)

## 例子
用unity打开工程，选择Ares/Examples菜单，打开的窗口里有所有的例子。
窗口底部有Goto Script按钮，可以看到对应的代码。

## 说明
实现IAresObjectV或IAresObjectH接口的ScriptableObject,Monobehavior或自定义的可以序列化的类，都可以使用AresInspector.

### Drawer
Drawer 是 所有绘制对象，如按钮，文字，下拉框等，这些比较简单，看一下例子代码即可。
Drawer 是 很容易扩展自己需要的。

### Config
Config 是 所有对Drawer的配置，如颜色，是否Enable等，也比较简单，看一下例子代码即可。
Config 是 很容易扩展自己需要的。

### Layout
Layout（布局） 主要是通过AresGroup和ACLayout配合控制的。<br/>
enum EAresGroupType  { Horizontal, Vertical, Foldout }<br/>
AresGroup : Attribute { int id; int parentId; EAresGroupType type; bool showBox;}<br/>
ACLayout : Attribute { int groupId; int order;}<br/>

继承于IAresObjectV或IAresObjectH接口的类, 默认都有个id为0的AresGroup。<br/>
如果想实现复杂的布局，就需要定义多个不同id和type的AresGroup，然后为每个Drawer通过ACLayout指定该Drawer所在的group以及绘制顺序, 参考ExampleAdjustOrder.

## 支持与联系
测试不够充分，目前只在2021.3.18f1测试过。<br/>
使用过程中有任何问题可以联系作者： QQ 94285060
