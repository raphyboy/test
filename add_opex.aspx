<%@ Page Language="VB" MasterPageFile="~/MasterPageMenu.master" AutoEventWireup="false" CodeFile="add_opex.aspx.vb" Inherits="add_opex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server"> 


    <div class="col-md-10">

            <ol class="breadcrumb headingbar">
            <li class="breadcrumb-item"><a href="Default.aspx">Default</a></li>
            <li class="breadcrumb-item active">Add Opex</li>
            <li class="breadcrumb-item"><a href="add_service.aspx">Add Service</a></li>
            <li class="breadcrumb-item"><a href="Summary.aspx">Summary</a></li>
            </ol>

       
            <div class="col-md-4">
                <asp:DropDownList ID="DropDownList1" runat="server" class="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <div class="input-group">
                <span class="input-group-addon"><asp:Label ID="Label1" runat="server" Text="จำนวน"></asp:Label></span>
                <asp:TextBox ID="TextBox4" runat="server" class="form-control">1</asp:TextBox>
                </div>
            </div>
            <asp:Button ID="Button1" runat="server" Text="เลือก" class="btn btn-success"/>
            <hr style="border-color: #DDD;" />
            <div class="col-md-10">
                <!--<table class="table" style="margin-top: -2%;">

                <tr>
                    <td>-->
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" GridLines="None"
                            BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" class="table">
                            <RowStyle BackColor="#f5f5f5" ForeColor="#337ab7" />
                            <Columns>
                                <asp:BoundField DataField="OPEX_Name" HeaderText="VAS &amp; Cost (per Month)" />
                                <asp:BoundField DataField="Number" HeaderText="จำนวน" />
                                <asp:TemplateField HeaderText="THB (exVAT)">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Cost") %>' CssClass="form-control"></asp:TextBox>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("id_List") %>' Visible="False"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowDeleteButton="True" />
                            </Columns>
                            <FooterStyle BackColor="#FFFFCC" ForeColor="#fff" />
                            <PagerStyle BackColor="#FFFFCC" ForeColor="#fff" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#fff" />
                            <HeaderStyle BackColor="#1abc9c" Font-Bold="True" ForeColor="white" />
                        </asp:GridView>
                        <asp:Button ID="Button3" runat="server" Text="Next" class="btn btn-success pull-right"/>
                        <asp:Button ID="Button2" runat="server" Text="บึนทึก" class="btn btn-success pull-right"/>
                        <asp:Button ID="Button4" runat="server" Text="Back" class="btn btn-success pull-right"/>
                    <!--</td>
                </tr>
            </table>-->
        
        </div>
        
        
        </div>
</asp:Content>    

