<%@ Page Title="Klasser" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="KlasseList.aspx.cs" Inherits="Timeplan.Lists.KlasseList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            
    <br />

    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Avbryt" CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonTop" />
    <asp:Button CssClass="btn btn-lg btn-default btn-float-right" runat="server" Text="Opprett ny klasse" OnClick="CreateNewButton_Click" ID="CreateNewButtonTop" />

    <section>
        <div>
            <hgroup>
                <h2><%: Page.Title %></h2>
            </hgroup>

            <asp:ListView
                ID="KlasseListView"
                runat="server"
                ItemPlaceholderID="KlasseItem"
                OnSorting="ListViewEvents_Sorting">

                <LayoutTemplate>
                    <table style="width: 100%;">
                        <tbody>
                            <tr class="listview-heading">
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Id" ID="IdLinkButton" runat="server" Visible="False">Id</asp:LinkButton>
                                    <div class="show-hide">
                                        <%--TODO: better descrition/view of show hide func--%>
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowIdLinkButton" Visible="False"></asp:LinkButton>
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Navn" ID="NavnLinkButton" runat="server">Navn</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowNavnLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="MaksAntallElever" ID="MaksAntallEleverLinkButton" runat="server">Maks antall elever</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowMaksAntallEleverLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Avdeling" ID="AvdelingLinkButton" runat="server">Avdeling</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowAvdelingLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Elever" ID="EleverLinkButton" runat="server">Elever</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowEleverLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                <th>
                                    <asp:LinkButton CommandName="Sort" CommandArgument="Ansatte" ID="AnsatteLinkButton" runat="server">Ansatte</asp:LinkButton>
                                    <div class="show-hide">
                                        <asp:LinkButton runat="server" OnClick="HideShowLinkButton_OnClick" Text="V/S" ID="HideShowAnsatteLinkButton"></asp:LinkButton>
                                    </div>
                                </th>
                                
                                <th></th>
                                <th></th>
                                <th></th>
                            </tr>
                            <tr id="KlasseItem" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>

                <EmptyItemTemplate>
                    <p>
                        Ingen elever funnet
                    </p>
                </EmptyItemTemplate>


                <ItemTemplate>
                    <tr class="row-standard">
                        <td>
                            <div class="form-group">
                                <asp:Button CssClass="form-control" runat="server" ID="IdButton" Text='<%# Eval("Id") %>' Visible="False"></asp:Button>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" runat="server" ID="NavnTextBox" MaxLength="100" Text='<%# Eval("Navn") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RequiredFieldValidator runat="server" ID="NavnRequiredFieldValidator" Display="Dynamic" ControlToValidate="NavnTextBox" ErrorMessage="Påkrevd" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:TextBox CssClass="form-control" runat="server" ID="MaksAntallEleverTextBox" Text='<%# Eval("MaksAntallElever") %>'></asp:TextBox>
                                <div class="error-message">
                                    <asp:RangeValidator runat="server" ID="MaksAntallEleverRangeValidator" ControlToValidate="MaksAntallEleverTextBox" Type="Double" MinimumValue="0" MaximumValue="100" Display="Dynamic" ErrorMessage="Ugyldig antall" />
                                    <asp:RequiredFieldValidator runat="server" ID="MaksAntallEleverRequiredFieldValidator" ControlToValidate="MaksAntallEleverTextBox" Display="Dynamic" ErrorMessage="Påkrevd" />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:DropDownList CssClass="form-control" runat="server" ID="AvdelingDropDown"></asp:DropDownList>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:CheckBoxList CssClass="form-control scrollingControlContainer" runat="server" Visible="True" ID="EleverListBox" Height="150"></asp:CheckBoxList>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group">
                                <asp:CheckBoxList CssClass="form-control scrollingControlContainer" runat="server" Visible="True" ID="AnsatteListBox" Height="150"></asp:CheckBoxList>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-save">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" CommandArgument='<%# Eval("Id") %>' OnClick="SaveButton_Click" ID="SaveLinkButton" ToolTip="Lagre endringer">
                                    <span class="glyphicon glyphicon-floppy-saved"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                            <div class="form-group btn-delete">
                                <asp:LinkButton CssClass="form-control btn btn-sm btn-default" runat="server" 
                                    OnClientClick='<%# String.Format("javascript:return ConfirmDelete(\"{0}\")", Eval("Navn") != null ? Eval("Navn").ToString() : string.Empty) %>' 
                                    CommandArgument='<%# Eval("Id") %>' OnClick="DeleteButton_Click" ID="DeleteLinkButton" ToolTip="Slett klasse">
                                    <span class="glyphicon glyphicon-remove"></span>
                                </asp:LinkButton>
                                <div class="correct-padding"></div>
                            </div>
                        </td>
                        <td>
                        
                        </td>
                    </tr>
                </ItemTemplate>

            </asp:ListView>
        
        </div>

    </section>

    <br />

    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Lagre alle" OnClick="SaveAllButton_Click" ID="SaveButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Avbryt" CausesValidation="false" OnClick="CancelButton_Click" ID="CancelButtonBottom" />
    <asp:Button CssClass="btn btn-lg btn-default" runat="server" Text="Opprett ny klasse" OnClick="CreateNewButton_Click" ID="CreateNewButtonBottom" />

    <script type="text/javascript" src="../Scripts/plugins/checkboxDropdown/bootstrap-multiselect.js"></script>
    <script type="text/javascript">

        //$(function () {
        //    $("[id*=EleverListBox]").multiselect({
        //        numberDisplayed: 3,
        //        maxHeight: 350,
        //        disableRemove: true
        //    });
        //});
        
        //$(function () {
        //    $("[id*=AnsatteListBox]").multiselect({
        //        numberDisplayed: 3,
        //        maxHeight: 350,
        //    });
        //});

        //var iterations = 100;
        //var totalTime = 0;

        //// Repeat the test the specified number of iterations.
        //for (i = 0; i < iterations; i++) {
        //    // Record the starting time, in UTC milliseconds.
        //    var start = new Date().getTime();

        //    // Execute the selector. The result does not need
        //    //  to be used or assigned to determine how long 
        //    //  the selector itself takes to run.
        //    $("select.jquery-search");

        //    // Record the ending time, in UTC milliseconds.
        //    var end = new Date().getTime();

        //    // Determine how many milliseconds elapsed and
        //    //  increment the test's totalTime counter.
        //    totalTime += (end - start);
        //}

        //// Report the average time taken by one iteration.
        //alert(totalTime / iterations);

        //$(function () {
        //    $("select.jquery-search").multiselect({
        //        numberDisplayed: 3,
        //        maxHeight: 350,
        //    });
        //});

        function ConfirmDelete(name) {
            if (confirm("Er du sikker på at du vil slette klasse '" + name + "'?")) {
                return true;
            }
            return false;
        }

        function ScrollToBottom() {
            window.scrollTo(0, document.body.scrollHeight);
        }

    </script>

</asp:Content>
