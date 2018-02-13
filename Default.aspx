<%@ Page Language="VB" MasterPageFile="~/MasterPageMenu.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>
        
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">            
            
        <div class="col-md-10">    
            <ol class="breadcrumb headingbar">
            <li class="breadcrumb-item active">Default</li><li class="breadcrumb-item"><a href="add_opex.aspx">Add Opex</a></li><li class="breadcrumb-item"><a href="add_service.aspx">Add Service</a></li><li class="breadcrumb-item"><a href="Summary.aspx">Summary</a></li></ol>
            
                <div class="col-md-10">
                    <asp:DropDownList ID="DropDownList1" runat="server" class="form-control">
                    </asp:DropDownList>
                </div>
                <asp:Button ID="Button1" runat="server" Text="เลือก" class="btn btn-success" style="margin-right: -56px;margin-top: -1px;"/>
                
                <hr style="border-color: #DDD;" />
            <div class="col-md-10">
            
                <!--<table class="table" style="margin-top: -5%;"> --> 
                            
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" GridLines="None"
                                BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" class="table">
                                <RowStyle BackColor="WhiteSmoke" ForeColor="#337AB7" />
                                <Columns>
                                    <asp:BoundField DataField="CAPEX_Name" HeaderText="Investment Cost (CAPEX)" />
                                    <asp:TemplateField HeaderText="Asset Type">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlAssetType" runat="server" CssClass="form-control" SelectedValue='<%# Bind("Asset_Type") %>'>
                                                <asp:ListItem>เช่าใช้</asp:ListItem>
                                                <asp:ListItem>เช่าซื้อ</asp:ListItem>
                                                <asp:ListItem>แถม</asp:ListItem>
                                            </asp:DropDownList>&nbsp;
                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("id_List") %>' Visible="False"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="THB (exVAT)">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control"></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Cost") %>' CssClass="form-control"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowDeleteButton="True" />
                                </Columns>
                                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                                <HeaderStyle BackColor="#1ABC9C" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>
                            <asp:Button ID="Button3" runat="server" Text="Next" class="btn btn-success pull-right"/>
                            <asp:Button ID="Button2" runat="server" Text="บึนทึก" class="btn btn-success pull-right"/>
                            <!--</table>-->
            </div> 
       </div>
</asp:Content>     
     

