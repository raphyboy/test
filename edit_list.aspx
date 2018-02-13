<%@ Page Language="VB" MasterPageFile="~/MasterPageMenu.master" AutoEventWireup="false" CodeFile="edit_list.aspx.vb" Inherits="edit_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">            
            
        <div class="col-md-10">    
            <ol class="breadcrumb headingbar">
            <li class="breadcrumb-item active">Edit</li>
            <!--<li class="breadcrumb-item"><a id="menu_opex" runat="server">Edit Opex</a></li>
            <li class="breadcrumb-item"><a id="menu_service" runat="server">Edit Service</a></li>
            <li class="breadcrumb-item"><a id="menu_summary" runat="server">Summary</a></li>-->
            </ol>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4">
                <Columns>
                    <asp:TemplateField HeaderText="เอกสาร">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Document_No") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:HyperLink ID="Link_DocumentID" runat="server" Text='<%# Bind("Document_No") %>'></asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Document_Date" HeaderText="วันที่จัดทำ" />
                    <asp:BoundField DataField="Area" HeaderText="Area" />
                    <asp:BoundField DataField="Cluster" HeaderText="Cluster" />
                    <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" />
                </Columns>
                <RowStyle BackColor="White" ForeColor="#330099" />
                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
            </asp:GridView>
            
        </div>
</asp:Content>  
