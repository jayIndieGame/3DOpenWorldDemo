## U3D开放世界Demo

> 该项目使用Unity2020.3LTS、Houdini18.5
>
> 重要的包：
>
> HoudiniEngineUnity.unitypackage
>
> Universal Render PineLine
>
> Cinemachine 2.6.5

## 项目概述

本项目从体验使用Houdini+Unity+URP出发。最终想要搭建出一个可以快速修改、迭代的OpenWorld。鉴于本人对其中各个技术模块并不了解。这个项目也是一个个人的学习项目。学习的文件也会归类放置在一定的目录下面。最终的成品Demo会放置在另一个目录下。后续有大文件的话也许会引入Git Lfs。目录也可能随着项目落实而变化。

现在这个项目里还是空空如也的。只是搭了个壳子。具体内容会一点点完善。

## 目录介绍

<details>
<summary>Assets</summary>
    <p>00_LearnSample</p>
    <p>01_OpenWorldDemo</p>
    <p>96_AddOns</p>
    <p>97_UtilClass</p>
    <p>98_PipeLineAssets</p>
    <p>99_Plugins</p>
</details>

- #### 00_LearnSample

  - 可能使用到的一些技术会做成小的Sample放在这个目录下边。方便记录和学习。

- #### 01_OpenWorldDemo

  - 最终成品放在这个目录下边。以及项目中涉及的所有内容。

- #### 96_AddOns

  - 全局会使用的一些模型、动画、纹理、材质等。

- #### 97_UtilClass

  - 工具类。静态类。

- #### 98_PipeLineAssets

  - URP的PipeLineAssets。

- #### 99_Plugins

  - 使用到的一些插件。Houdini For Unity、Dotween等。

## 项目目标

今后的**TODO** **List**：

##### 第一阶段：

- [ ] 3D场景使用Houdini搭建。并且实现Hda对场景快速迭代。
- [ ] 3D基本的Character Controller、人物动画状态机。
- [ ] 让游戏跑起来。

##### 第二阶段：

- [ ] 实现场景交互、材质破碎。
- [ ] 实现部分特效VFX
- [ ] 实现特殊效果的渲染。例如给出水流（焦散）、体积云\雾等物体在URP下的光栅化解决方案。
- [ ] 性能优化。让整个方案具备一定落地的可能性。

