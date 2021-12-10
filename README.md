#方案一：WebApi和Web在同一项目内，前后端不分离，使用简单的前端框架，用以减少开发时间。（本项目）
## wfms
Work flow manage system

## 项目设计
使用ASP.NET Core WebApi + MVC 实现接口和WebSite
数据库使用MS SQLServer
开发工具 Visual Studio 2022

## 框架使用
ASP.NET Core 6.0
AspNetCore.Mvc.Razor 6.0
EntityFrameworkCore 6.0
Swashbuckle.AspNetCore

## 前端使用
Jquery
Bootstrap
Bootswtch

## 示例地址
WebApi ： https://localhost:44312/
WebSite： https://localhost:44312/web

## WebApi
EmployeesController 仅对与Employee进行CURD
TasksController 仅对于Task进行CURD
EmployeesGenController 对于员工信息和员工任务进行CURD
V_EmployeeTasksController 列举全部员工和对应任务信息

#方案二：前后端分离，后端使用WebApi接口，前端使用ElementUI + Vue。开发成本略高。