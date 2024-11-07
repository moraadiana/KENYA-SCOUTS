<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="StoreListing.aspx.cs" Inherits="KSAStaff.pages.StoreListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
     <div class="content-wrapper">
        <section class="content-header">
            <h1>Store Requests</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Store Requests</li>
            </ol>
        </section>

        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-diamond"></i>&nbsp;Store Request</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse"><i class="fa fa-minus"></i></button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-remove"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <p class="text-center"><a class="btn btn-pill btn-info u-posRelative" href="StoreLines.aspx?query=new&status=Open">New Stores Requisition<span class="waves"></span> </a></p>
                                    <br />
                                    <div class="col-md-12">
                                        <div class="table-responsive">
                                            <table id="example1" class="table no-margin">
                                                <thead>
                                                    <tr>
                                                        <th class="small">#No</th>
                                                        <th class="small">Requisition Type</th>
                                                        <th class="small">Request Date</th>
                                                        <th class="small">Required Date</th>
                                                        <th class="small">Status</th>
                                                        <th class="small">Actions</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                   <%=Jobs() %>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
</asp:Content>
