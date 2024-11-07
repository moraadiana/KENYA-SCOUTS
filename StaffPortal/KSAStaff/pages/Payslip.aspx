<%@ Page Title="Payslip" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="Payslip.aspx.cs" Inherits="KSAStaff.pages.Payslip" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
     <div class="content-wrapper">
        <section class="content-header">
            <h1>Payslip</h1>
            <ol class="breadcrumb">
                <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Payslip</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">Individual payslip</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <div class="btn-group">
                                    <button type="button" class="btn btn-box-tool dropdown-toggle" data-toggle="dropdown">
                                        <i class="fa fa-wrench"></i>
                                    </button>
                                    <ul class="dropdown-menu" role="menu">
                                    </ul>
                                </div>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="inputEmail3" class="col-sm-2 control-label">Year:</label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlYear" class="form-control select2" runat="server" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <label for="inputEmail3" class="col-sm-2 control-label">Month</label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList ID="ddlMonth" class="form-control select2" runat="server" OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <%--<div class="col-sm-2">
                                        <button type="button" class="btn btn-block btn-primary"><i class="fa fa-file-pdf-o"></i>View payslip</button>
                                    </div>--%>
                                </div>
                            </div>
                            <span style="font-size: 10pt">
                                <iframe runat="server" id="myPDF" src="" width="100%" height="500" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
