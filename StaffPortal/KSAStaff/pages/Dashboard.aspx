<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Layouts/Main.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="KSAStaff.pages.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Main1" runat="server">
    
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Dashboard</h1>
            <ol class="breadcrumb">
                <li><a href="#"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Dashboard</li>
            </ol>
        </section>
        <!-- Main content -->
        <section class="content">
            <!-- Info boxes -->
            <div class="row">

                <div class="col-md-12" style="display: none">
                    <iframe src="10.10.1.140:8080/BC200/?company=ARA&page=22&showribbon=0&shownavigation=0&showuiparts=0" width="100%" height="500px" webserver=""></iframe>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="info-box bg-green">
                        <span class="info-box-icon"><i class="ion ion-ios-pricetag-outline"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Leave Approvals</span>
                            <%--<span class="info-box-number"><%=TotalLeave() %></span>--%>
                            <span class="info-box-number">0</span>

                            <div class="progress">
                                <div class="progress-bar" style="width: 50%"></div>
                            </div>
                            <a href="#" class="small-box-footer white" style="color: white">View Requests <i class="fa fa-arrow-circle-right"></i></a>
                            <%--  <span class="progress-description">50% Increase in 30 Days
                  </span>--%>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="info-box bg-yellow">
                        <span class="info-box-icon"><i class="ion ion-android-list"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Imprest Approvals</span>
                            <%--<span class="info-box-number"><%=TotalImprest() %></span>--%>
                            <span class="info-box-number">0</span>

                            <div class="progress">
                                <div class="progress-bar" style="width: 20%"></div>
                            </div>
                            <a href="#" class="small-box-footer" style="color: white">View Requests <i class="fa fa-arrow-circle-right"></i></a>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->

                <!-- fix for small devices only -->
                <div class="clearfix visible-sm-block"></div>

                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="info-box bg-red">
                        <span class="info-box-icon"><i class="ion ion-jet"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Claim Approvals</span>
                            <%--<span class="info-box-number"><%=TotalClaim() %></span>--%>
                            <span class="info-box-number">0</span>

                            <div class="progress">
                                <div class="progress-bar" style="width: 70%"></div>
                            </div>
                            <a href="#" class="small-box-footer white" style="color: white">View Requests <i class="fa fa-arrow-circle-right"></i></a>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="info-box bg-aqua">
                        <span class="info-box-icon"><i class="ion ion-ios-email-outline"></i></span>

                        <div class="info-box-content">
                            <span class="info-box-text">Stores Approvals</span>
                            <%--<span class="info-box-number"><%=TotalStores() %></span>--%>
                            <span class="info-box-number">0</span>

                            <div class="progress">
                                <div class="progress-bar" style="width: 40%"></div>
                            </div>
                            <a href="#" class="small-box-footer white" style="color: white">View Requests <i class="fa fa-arrow-circle-right"></i></a>
                        </div>
                        <!-- /.info-box-content -->
                    </div>
                    <!-- /.info-box -->
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->

            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-header with-border">
                            <h3 class="box-title">User Profile</h3>

                            <div class="box-tools pull-right">
                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                    <i class="fa fa-minus"></i>
                                </button>
                                <div class="btn-group">

                                    <ul class="dropdown-menu" role="menu">
                                    </ul>
                                </div>
                                <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                            </div>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <div class="row">
                                <!-- /.col -->
                                <div class="col-md-4">
                                    <p class="text-center">
                                        <strong>User Avatar </strong>
                                    </p>
                                    <div class="box-body box-profile">
                                        <asp:Image ID="ImgProfileDefault" class="profile-user-img img-responsive img-circle" runat="server" Height="250px" Width="200px" Visible="false" alt="User profile picture" />
                                        <asp:Image ID="ImgProfilePic" class="profile-user-img img-responsive img-circle" runat="server" Height="250px" Width="200px" alt="User profile picture" />
                                        <%--<img class="profile-user-img img-responsive img-circle" src="../../dist/img/user4-128x128.jpg" alt="User profile picture">--%>
                                        <h3 class="profile-username text-center">
                                            <asp:Label ID="lblTitle" runat="server" Text="" Visible="false"></asp:Label>
                                            <asp:Label ID="lblStaffName" runat="server" Text=""></asp:Label></h3>
                                        <p class="text-muted text-center">
                                            <asp:Label ID="lblDesignation" runat="server" Text=""></asp:Label>
                                        </p>
                                        <%--<a href="#" class="btn btn-primary btn-block"><b>Follow</b></a>--%>
                                    </div>
                                    <div class="progress-group">
                                        <span class="progress-text"></span>
                                        <span class="progress-number"><b></b></span>

                                        <div class="progress sm">
                                            <div class="progress-bar progress-bar-success" style="width: 100%"></div>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.col -->
                                <div class="col-md-8">
                                    <p class="text-center">
                                        <strong>Personal Information</strong>
                                    </p>

                                    <ul class="list-group list-group-unbordered">
                                        <li class="list-group-item">
                                            <b>Employee No:</b> <a class="pull-right">
                                                <asp:Label ID="lblEmployeeNo" runat="server" Text=""></asp:Label></a>
                                        </li>
                                        <li class="list-group-item">
                                            <b>Gender:</b> <a class="pull-right">
                                                <asp:Label ID="lblGender" runat="server" Text=""></asp:Label></a>
                                        </li>
                                        <li class="list-group-item">
                                            <b>ID Number:</b> <a class="pull-right">
                                                <asp:Label ID="lblIDNo" runat="server" Text=""></asp:Label></a>
                                        </li>
                                        <li class="list-group-item">
                                            <b>Email Address:</b> <a class="pull-right">
                                                <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></a>
                                        </li>
                                        <li class="list-group-item">
                                            <b>Company E-Mail:</b> <a class="pull-right">
                                                <asp:Label ID="lblEmailCompany" runat="server" Text=""></asp:Label></a>
                                        </li>
                                        <li class="list-group-item">
                                            <b>Phone Number:</b> <a class="pull-right">
                                                <asp:Label ID="lblPhoneNo" runat="server" Text=""></asp:Label></a>
                                        </li>
                                        <li class="list-group-item">
                                            <b>Citizenship:</b> <a class="pull-right">
                                                <asp:Label ID="lblCitizenship" runat="server" Text=""></asp:Label></a>
                                        </li>
                                        <li class="list-group-item">
                                            <b>Postal Address:</b> <a class="pull-right">
                                                <asp:Label ID="lblPostalCode" runat="server" Text=""></asp:Label>
                                                <asp:Label ID="lblPostalAddress" runat="server" Text=""></asp:Label></a>
                                        </li>
                                    </ul>
                                    <div class="box-footer clearfix">
                                    </div>
                                </div>
                            </div>
                            <!-- /.row -->
                        </div>
                    </div>
                    <!-- /.box -->
                </div>
                <!-- /.col -->
            </div>
        </section>
        <!-- /.content -->
    </div>
</asp:Content>

