<%@ Page Title="Memo Report" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="MemoReport.aspx.cs" Inherits="KSAStaff.pages.MemoReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Memo Report</h1>
            <ol class="breadcrumb">
                <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Memo Report</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-primary box-solid">
                        <div class="box-header with-border">
                            <h3 class="box-title">Memo Report</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <div class="btn-group">
                                    <button type="button" class="btn btn-box-tool dropdown-toggle" data-toggle="dropdown">
                                        <i class="fa fa-wrench"></i>
                                    </button>
                                    <ul class="dropdown-menu" role="menu"></ul>
                                </div>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <span style="font-size: 10pt">
                                <!--<embed runat="server" type="application/pdf" width="100%" height="500" src="//10.107.8.40/Downloads/MemoReport-NCIA00001.pdf" />-->
                                <iframe runat="server" id="myPDF" src="" width="100%" height="500" />
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </section>
</asp:Content>
