#pragma checksum "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2dd526462ad6ee58c50b8d876873a7448b1988ca"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Admin_Index), @"mvc.1.0.view", @"/Views/Admin/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using AppEducation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using AppEducation.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using AppEducation.Models.Users;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using System.Linq;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2dd526462ad6ee58c50b8d876873a7448b1988ca", @"/Views/Admin/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"ca2f929184fdcfbd5686be2d8022df2e892040ba", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<AppUser>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-inline ml-3"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/dist/img/avataAdmin"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("User Avatar"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("img-size-50 mr-3 img-circle"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("img-size-50 img-circle mr-3"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/dist/img/AvataAdmin2.jpg"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("img-circle elevation-2"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("alt", new global::Microsoft.AspNetCore.Html.HtmlString("User Image"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-sm btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_10 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_11 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_12 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("hold-transition sidebar-mini layout-fixed"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
  
  Layout = "_LayoutAdmin";
  ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("body", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2dd526462ad6ee58c50b8d876873a7448b1988ca8947", async() => {
                WriteLiteral(@"
<div class=""wrapper"">

  <!-- Navbar -->
  <nav class=""main-header navbar navbar-expand navbar-white navbar-light"">
    <!-- Left navbar links -->
    <ul class=""navbar-nav"">
      <li class=""nav-item"">
        <a class=""nav-link"" data-widget=""pushmenu"" href=""#"" role=""button""><i class=""fas fa-bars""></i></a>
      </li>
      <li class=""nav-item d-none d-sm-inline-block"">
        <a href=""index3.html"" class=""nav-link"">Home</a>
      </li>
      <li class=""nav-item d-none d-sm-inline-block"">
        <a href=""#"" class=""nav-link"">");
#nullable restore
#line 21 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                                Write(ViewData["Title"]);

#line default
#line hidden
#nullable disable
                WriteLiteral("</a>\r\n      </li>\r\n    </ul>\r\n\r\n    <!-- SEARCH FORM -->\r\n    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2dd526462ad6ee58c50b8d876873a7448b1988ca10105", async() => {
                    WriteLiteral(@"
      <div class=""input-group input-group-sm"">
        <input class=""form-control form-control-navbar"" type=""search"" placeholder=""Search"" aria-label=""Search"">
        <div class=""input-group-append"">
          <button class=""btn btn-navbar"" type=""submit"">
            <i class=""fas fa-search""></i>
          </button>
        </div>
      </div>
    ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"

    <!-- Right navbar links -->
    <ul class=""navbar-nav ml-auto"">
      <!-- Messages Dropdown Menu -->
      <li class=""nav-item dropdown"">
        <a class=""nav-link"" data-toggle=""dropdown"" href=""#"">
          <i class=""far fa-comments""></i>
          <span class=""badge badge-danger navbar-badge"">3</span>
        </a>
        <div class=""dropdown-menu dropdown-menu-lg dropdown-menu-right"">
          <a href=""#"" class=""dropdown-item"">
            <!-- Message Start -->
            <div class=""media"">
              ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "2dd526462ad6ee58c50b8d876873a7448b1988ca12421", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
              <div class=""media-body"">
                <h3 class=""dropdown-item-title"">
                  Nguyen Manh Cuong
                  <span class=""float-right text-sm text-danger""><i class=""fas fa-star""></i></span>
                </h3>
                <p class=""text-sm"">User Error</p>
                <p class=""text-sm text-muted""><i class=""far fa-clock mr-1""></i> 4 Hours Ago</p>
              </div>
            </div>
            <!-- Message End -->
          </a>
          <div class=""dropdown-divider""></div>
          <a href=""#"" class=""dropdown-item"">
            <!-- Message Start -->
            <div class=""media"">
              ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "2dd526462ad6ee58c50b8d876873a7448b1988ca14371", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
              <div class=""media-body"">
                <h3 class=""dropdown-item-title"">
                  Nguyen Quoc Khanh
                  <span class=""float-right text-sm text-muted""><i class=""fas fa-star""></i></span>
                </h3>
                <p class=""text-sm"">I got your message bro</p>
                <p class=""text-sm text-muted""><i class=""far fa-clock mr-1""></i> 4 Hours Ago</p>
              </div>
            </div>
            <!-- Message End -->
          </a>
          <div class=""dropdown-divider""></div>
          <a href=""#"" class=""dropdown-item"">
            <!-- Message Start -->
            <div class=""media"">
              ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "2dd526462ad6ee58c50b8d876873a7448b1988ca16332", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
              <div class=""media-body"">
                <h3 class=""dropdown-item-title"">
                  Nguyen Vinh Hien
                  <span class=""float-right text-sm text-warning""><i class=""fas fa-star""></i></span>
                </h3>
                <p class=""text-sm"">The subject goes here</p>
                <p class=""text-sm text-muted""><i class=""far fa-clock mr-1""></i> 4 Hours Ago</p>
              </div>
            </div>
            <!-- Message End -->
          </a>
          <div class=""dropdown-divider""></div>
          <a href=""#"" class=""dropdown-item dropdown-footer"">See All Messages</a>
        </div>
      </li>
      <!-- Notifications Dropdown Menu -->
      <li class=""nav-item dropdown"">
        <a class=""nav-link"" data-toggle=""dropdown"" href=""#"">
          <i class=""far fa-bell""></i>
          <span class=""badge badge-warning navbar-badge"">15</span>
        </a>
        <div class=""dropdown-menu dropdown-menu-lg dropdown-menu-right"">
          <span class=""");
                WriteLiteral(@"dropdown-item dropdown-header"">15 Notifications</span>
          <div class=""dropdown-divider""></div>
          <a href=""#"" class=""dropdown-item"">
            <i class=""fas fa-envelope mr-2""></i> 4 new messages
            <span class=""float-right text-muted text-sm"">3 mins</span>
          </a>
          <div class=""dropdown-divider""></div>
          <a href=""#"" class=""dropdown-item"">
            <i class=""fas fa-users mr-2""></i> 8 friend requests
            <span class=""float-right text-muted text-sm"">12 hours</span>
          </a>
          <div class=""dropdown-divider""></div>
          <a href=""#"" class=""dropdown-item"">
            <i class=""fas fa-file mr-2""></i> 3 new reports
            <span class=""float-right text-muted text-sm"">2 days</span>
          </a>
          <div class=""dropdown-divider""></div>
          <a href=""#"" class=""dropdown-item dropdown-footer"">See All Notifications</a>
        </div>
      </li>
      <li class=""nav-item"">
        <a class=""nav-link"" data-widg");
                WriteLiteral(@"et=""fullscreen"" href=""#"" role=""button"">
          <i class=""fas fa-expand-arrows-alt""></i>
        </a>
      </li>
      <li class=""nav-item"">
        <a class=""nav-link"" data-widget=""control-sidebar"" data-slide=""true"" href=""#"" role=""button"">
          <i class=""fas fa-th-large""></i>
        </a>
      </li>
    </ul>
  </nav>
  <!-- /.navbar -->

  <!-- Main Sidebar Container -->
  <aside class=""main-sidebar sidebar-dark-primary elevation-4"">
    <!-- Sidebar -->
    <div class=""sidebar"">
      <!-- Sidebar user panel (optional) -->
      <div class=""user-panel mt-3 pb-3 mb-3 d-flex"">
        <div class=""image"">
          ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "2dd526462ad6ee58c50b8d876873a7448b1988ca20469", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
        </div>
        <div class=""info"">
          <a href=""#"" class=""d-block"">Cuong Top</a>
        </div>
      </div>

      <!-- SidebarSearch Form -->
      <div class=""form-inline"">
        <div class=""input-group"" data-widget=""sidebar-search"">
          <input class=""form-control form-control-sidebar"" type=""search"" placeholder=""Search"" aria-label=""Search"">
          <div class=""input-group-append"">
            <button class=""btn btn-sidebar"">
              <i class=""fas fa-search fa-fw""></i>
            </button>
          </div>
        </div>
      </div>

      <!-- Sidebar Menu -->
      <nav class=""mt-2"">
        <ul class=""nav nav-pills nav-sidebar flex-column"" data-widget=""treeview"" role=""menu"" data-accordion=""false"">
          <!-- Add icons to the links using the .nav-icon class
               with font-awesome or any other icon font library -->
          <li class=""nav-item menu-open"">
            <a href=""#"" class=""nav-link active"">
              <i class=""nav-ico");
                WriteLiteral(@"n fas fa-tachometer-alt""></i>
              <p>
                Dashboard
                <i class=""right fas fa-angle-left""></i>
              </p>
            </a>
            <ul class=""nav nav-treeview"">
              <li class=""nav-item"">
                <a href=""./index.html"" class=""nav-link active"">
                  <i class=""far fa-circle nav-icon""></i>
                  <p>Dashboard v1</p>
                </a>
              </li>
              <li class=""nav-item"">
                <a href=""./index2.html"" class=""nav-link"">
                  <i class=""far fa-circle nav-icon""></i>
                  <p>Dashboard v2</p>
                </a>
              </li>
            </ul>
          </li>
       
        </ul>
      </nav>
      <!-- /.sidebar-menu -->
    </div>
    <!-- /.sidebar -->
  </aside>

  <!-- Content Wrapper. Contains page content -->
  <div class=""content-wrapper"">
    <!-- Content Header (Page header) -->
    <div class=""content-header"">
      <div clas");
                WriteLiteral(@"s=""container-fluid"">
        <div class=""row mb-2"">
          <div class=""col-sm-6"">
            <h1 class=""m-0"">Dashboard</h1>
          </div><!-- /.col -->
          <div class=""col-sm-6"">
            <ol class=""breadcrumb float-sm-right"">
              <li class=""breadcrumb-item""><a href=""#"">Home</a></li>
              <li class=""breadcrumb-item active"">Dashboard v1</li>
            </ol>
          </div><!-- /.col -->
        </div><!-- /.row -->
      </div><!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->

    <!-- Main content -->
    <section class=""content"" id=""section-content"">
      <div class=""container-fluid"">
        <!-- Small boxes (Stat box) -->
        <div class=""row"">
        <div class=""col-lg-3 col-6"">
            <!-- small box -->
            <div class=""small-box bg-warning"">
            <div class=""inner"">
                <h3>");
#nullable restore
#line 229 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
               Write(Model.Count());

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</h3>

                <p>User Registrations</p>
            </div>
            <div class=""icon"">
                <i class=""ion ion-person-add""></i>
            </div>
            <a href=""#"" id=""UserRegis"" class=""small-box-footer"">More info <i class=""fas fa-arrow-circle-right""></i></a>
            </div>
        </div>
        <!-- ./col -->
        <div class=""col-lg-3 col-6"">
            <!-- small box -->
            <div class=""small-box bg-success"">
            <div class=""inner"">
                <h3>53<sup style=""font-size: 20px""></sup></h3>

                <p>Student Online</p>
            </div>
            <div class=""icon"">
                <i class=""ion ion-stats-bars""></i>
            </div>
            <a href=""#"" class=""small-box-footer"">More info <i class=""fas fa-arrow-circle-right""></i></a>
            </div>
        </div>
        <!-- ./col -->
        <div class=""col-lg-3 col-6"">
            <!-- small box -->
            <div class=""small-box bg-info"">
      ");
                WriteLiteral(@"      <div class=""inner"">
                <h3>53<sup style=""font-size: 20px""></sup></h3>

                <p>Teacher Online</p>
            </div>
            <div class=""icon"">
                <i class=""ion ion-stats-bars""></i>
            </div>
            <a href=""#"" class=""small-box-footer"">More info <i class=""fas fa-arrow-circle-right""></i></a>
            </div>
        </div>
        <!-- ./col -->
        <div class=""col-lg-3 col-6"">
            <!-- small box -->
            <div class=""small-box bg-danger"">
            <div class=""inner"">
                <h3>65</h3>

                <p>Class Online</p>
            </div>
            <div class=""icon"">
                <i class=""ion ion-pie-graph""></i>
            </div>
            <a href=""#"" class=""small-box-footer"">More info <i class=""fas fa-arrow-circle-right""></i></a>
            </div>
        </div>
        <!-- ./col -->
        </div>
      </div>
    </section>

    <div class=""content-wrapper"" id=""table-user");
                WriteLiteral(@"""  style=""margin-right: 1px; display:none ; margin-left:1px ; width:100%"">
    <!-- Main content -->
    <section class=""content"">
      <div class=""container-fluid"">
        <div class=""row"">
          <div class=""col-12"">
            <div class=""card"">
              <div class=""card-header"">
                <h3 class=""card-title"">Danh Sách Người Dùng</h3>
              </div>
              <!-- /.card-header -->
              <div class=""card-body"">
                <table id=""example2"" class=""table table-bordered table-hover"">
                  <thead>
                  <tr>
                    <th>STT</th>
                    <th>ID</th>
                    <th>User Name</th>
                    <th>Email</th>
                    <th>Password</th>
                    <th>Modify</th>
                  </tr>
                  </thead>
                  <tbody>
");
#nullable restore
#line 313 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                     if(Model.Count()==0)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                     <tr><td> No user registered </td></tr> \r\n");
#nullable restore
#line 316 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                    }else{
                      var i = 0;
                      foreach(AppUser user in Model){

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <tr>\r\n                          <td>");
#nullable restore
#line 320 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                         Write(i);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                          <td>");
#nullable restore
#line 321 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                         Write(user.Id);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                          <td>");
#nullable restore
#line 322 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                         Write(user.UserName);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                          <td>");
#nullable restore
#line 323 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                         Write(user.Email);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                          <td>");
#nullable restore
#line 324 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                         Write(user.PasswordHash);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                          <td>\r\n                              ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2dd526462ad6ee58c50b8d876873a7448b1988ca30332", async() => {
                    WriteLiteral("\r\n                                  ");
                    __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "2dd526462ad6ee58c50b8d876873a7448b1988ca30633", async() => {
                        WriteLiteral("Edit");
                    }
                    );
                    __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                    __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                    __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_8);
                    __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_9.Value;
                    __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_9);
                    if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
                    {
                        throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
                    }
                    BeginWriteTagHelperAttribute();
#nullable restore
#line 328 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                                       WriteLiteral(user.Id);

#line default
#line hidden
#nullable disable
                    __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                    __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
                    __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                    await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                    if (!__tagHelperExecutionContext.Output.IsContentModified)
                    {
                        await __tagHelperExecutionContext.SetOutputContentAsync();
                    }
                    Write(__tagHelperExecutionContext.Output);
                    __tagHelperExecutionContext = __tagHelperScopeManager.End();
                    WriteLiteral("\r\n                                  <button type=\"submit\"\r\n                                          class=\"btn btn-sm btn-danger\">\r\n                                      Delete\r\n                                  </button>\r\n                              ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_10.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_10);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper", "RouteValues"));
                }
                BeginWriteTagHelperAttribute();
#nullable restore
#line 326 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                                                          WriteLiteral(user.Id);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_11.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_11);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                          </td>\r\n                        </tr>\r\n");
#nullable restore
#line 336 "E:\WebEducation2020\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
          
                      }
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"             
                  </tbody>
                 
                </table>
              </div>
              <!-- /.card-body -->
            </div>
            <!-- /.card -->
          </div>
          <!-- /.col -->
        </div>
        <!-- /.row -->
      </div>
      <!-- /.container-fluid -->
    </section>
    <!-- /.content -->
  </div>
  <!-- /.content-wrapper -->
  </div>
       
</div>
  <!-- /.content-wrapper -->
  <footer class=""main-footer"">
    <strong> MTA Education 2020 <a href=""#"">facebook</a>.</strong>
    <div class=""float-right d-none d-sm-inline-block"">
      <b>Version</b> 0.1
    </div>
  </footer>
  <!-- Control Sidebar -->
  <aside class=""control-sidebar control-sidebar-dark"">
    <!-- Control sidebar content goes here -->
  </aside>
  <!-- /.control-sidebar -->
</div>
<!-- ./wrapper -->
");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.BodyTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_BodyTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_12);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<AppUser>> Html { get; private set; }
    }
}
#pragma warning restore 1591
