<%@ Page Title="Leave Listing" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="LeaveListing.aspx.cs" Inherits="KSAStaff.pages.LeaveListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
     <div class="content-wrapper">
        <section class="content-header">
            <h1>Leave Listing</h1>
            <ol class="breadcrumb">
                <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Leave Listing</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box box-info">
                        <div class="box-header with-border">
                            <h3 class="box-title"><i class="fa fa-server"></i>&nbsp;My Leave Request</h3>
                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <div class="box-body">
                            <div class="col-md-12">
                                <p class="text-center"><a class="btn btn-pill btn-info u-posRelative pull-left" href="LeaveApplication.aspx">Apply For Leave<span class="waves"></span> </a></p>
                                <%--<a class="btn btn-pill btn-warning u-posRelative pull-right" href="LeaveStatement.aspx"><i class="fa fa-file-pdf-o"></i>My Leave Statement<span class="waves"></span> </a>--%>
                            </div>
                            <br />
                            <br />
                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <table id="example1" class="table no-margin">
                                        <thead>
                                            <tr>
                                                <th class="small">#</th>
                                                <th class="small">Ref No</th>
                                                <th class="small">Code</th>
                                                <th class="small">Days Applied</th>
                                                <th class="small">Application Date</th>
                                                <th class="small">Start</th>
                                                <th class="small">End</th>
                                                <th class="small">Return</th>
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
        </section>
    </div>
</asp:Content>
