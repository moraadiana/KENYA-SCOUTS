<%@ Page Title="" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="PettyCashListing.aspx.cs" Inherits="KSAStaff.pages.PettyCashListing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    <div class="content-wrapper">
    <section class="content-header">
        <h1>Petty Cash Listing
        </h1>
        <ol class="breadcrumb">
            <li><a href="Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
            <li class="active">Petty Cash Listing</li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="box box-info box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">My Petty Cash Listing</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>

                    <div class="box-body">
                        <p class="text-center"><a class="btn btn-pill btn-danger u-posRelative" href="PettyCashLines.aspx?query=new&status=Open">New Petty Cash<span class="waves"></span> </a></p>
                        <br />
                        <div class="table-responsive">
                            <table id="example1" class="table no-margin">
                                <thead>
                                    <tr>
                                        <th class="small">#No</th>
                                        <th class="small">Payee</th>
                                        <th class="small">Purpose</th>
                                        <th class="small">Status</th>
                                        <th class="small">Actions</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%=Jobs()%>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</div>
</asp:Content>
