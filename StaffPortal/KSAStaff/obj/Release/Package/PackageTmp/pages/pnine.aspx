<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="Pnine.aspx.cs" Inherits="KSAStaff.pages.Pnine" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
     <div class="content-wrapper">
     <!-- Content Header (Page header) -->
     <section class="content-header">
         <h1>Staff P9 form
    
             <%--<small>Version 2.0</small>--%>
         </h1>
         <ol class="breadcrumb">
             <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
             <li class="active">Staff P9 form</li>
         </ol>
     </section>

     <!-- Main content -->
     <section class="content">
         <div class="row">
             <div class="col-md-12">
                 <div class="box box-primary">
                     <div class="box-header with-border">
                         <h3 class="box-title">Individual P9 form</h3>

                         <div class="box-tools pull-right">
                             <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                 <i class="fa fa-minus"></i>
                             </button>
                             <div class="btn-group">
                                 <button type="button" class="btn btn-box-tool dropdown-toggle" data-toggle="dropdown">
                                     <i class="fa fa-wrench"></i>
                                 </button>
                                 <ul class="dropdown-menu" role="menu">
                                     <%-- <li><a href="#">Action</a></li>
                                     <li><a href="#">Another action</a></li>
                                     <li><a href="#">Something else here</a></li>
                                     <li class="divider"></li>
                                     <li><a href="#">Separated link</a></li>--%>
                                 </ul>
                             </div>
                             <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                         </div>
                     </div>
                     <div class="box-body">
                         <span style="font-size: 12pt; color: green; font-family: Palatino Linotype">Please, select the period to view the p9 for that period</span><br />
                         <div class="form-horizontal">
                             <table id="table1" style="width: 100%;">
                                 <tr>
                                     <td class="auto-style1">
                                         <asp:Label ID="Label11" runat="server" Text="Period Year:"></asp:Label>
                                     </td>
                                     <td colspan="3" class="auto-style1">
                                         <asp:DropDownList ID="ddlYear" runat="server" Width="233px" CssClass="form-control select2" AutoPostBack="True" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                                         </asp:DropDownList>&nbsp;
                                     </td>
                                 </tr>
                             </table>
                         </div>
                         <span style="font-size: 10pt">
                             <iframe runat="server" id="myPDF" src="" width="100%" height="500" />
                         </span>
                     </div>
                 </div>
                 <!-- /.box -->
             </div>
             <!-- /.col -->
         </div>
     </section>
 </div>
</asp:Content>
