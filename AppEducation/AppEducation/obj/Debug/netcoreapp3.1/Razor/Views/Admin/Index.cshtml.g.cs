#pragma checksum "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "69d4fe504a297451ac2ae96421d17e547ad34460"
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
#line 1 "D:\SQl\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using AppEducation;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\SQl\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using AppEducation.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "D:\SQl\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using AppEducation.Models.Users;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "D:\SQl\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
#nullable disable
#nullable restore
#line 5 "D:\SQl\Project\AppEducation\AppEducation\Views\_ViewImports.cshtml"
using System.Linq;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"69d4fe504a297451ac2ae96421d17e547ad34460", @"/Views/Admin/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3fc49cbcc4d653b572bf984153d351be992c8d02", @"/Views/_ViewImports.cshtml")]
    public class Views_Admin_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<AppUser>>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-sm btn-primary"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Delete", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 2 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
  
  Layout = "_LayoutAdmin";
  ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
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
#line 16 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
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
          <div class=""inner"">
            <h3>53<sup style=""font-size: 20px""></");
            WriteLiteral(@"sup></h3>

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

<div class=""content-wrapper"" id=""table-user"" style=""margin-right: 1px; display:none ; margin-left:1px ; width:100%"">
  <!-- Main content -->
  <section class=""content"">
    <div class=""container-fluid");
            WriteLiteral(@""">
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
#line 100 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                   if (Model.Count() == 0)
                  {

#line default
#line hidden
#nullable disable
            WriteLiteral("                    <tr>\r\n                      <td> No user registered </td>\r\n                    </tr>\r\n");
#nullable restore
#line 105 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                  }
                  else
                  {
                    var i = 0;
                    foreach (AppUser user in Model)
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                      <tr>\r\n                        <td>");
#nullable restore
#line 112 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                       Write(i);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 113 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                       Write(user.Id);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 114 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                       Write(user.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 115 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                       Write(user.Email);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td>");
#nullable restore
#line 116 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                       Write(user.PasswordHash);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td>\r\n                          ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "69d4fe504a297451ac2ae96421d17e547ad3446010846", async() => {
                WriteLiteral("\r\n                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "69d4fe504a297451ac2ae96421d17e547ad3446011133", async() => {
                    WriteLiteral("Edit");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
                {
                    throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
                }
                BeginWriteTagHelperAttribute();
#nullable restore
#line 119 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
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
                WriteLiteral("\r\n                            <button type=\"submit\" class=\"btn btn-sm btn-danger\">\r\n                              Delete\r\n                            </button>\r\n                          ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
#nullable restore
#line 118 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
                                                      WriteLiteral(user.Id);

#line default
#line hidden
#nullable disable
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                        </td>\r\n                      </tr>\r\n");
#nullable restore
#line 126 "D:\SQl\Project\AppEducation\AppEducation\Views\Admin\Index.cshtml"
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
<!-- /.content-wrapper -->
");
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
