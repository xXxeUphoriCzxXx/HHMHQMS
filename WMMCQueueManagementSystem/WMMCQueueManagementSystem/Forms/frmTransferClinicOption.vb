Public Class frmTransferClinicOption

    Public isTranferHold As Integer

    Sub New()
        InitializeComponent()
    End Sub

    Private Sub btnTranferToClinic_Click(sender As Object, e As EventArgs) Handles btnTranferToClinic.Click
        isTranferHold = 0
        Me.DialogResult = DialogResult.Yes
        Close()
    End Sub

    Private Sub btnTranferAndHold_Click(sender As Object, e As EventArgs) Handles btnTranferAndHold.Click
        isTranferHold = 1
        Me.DialogResult = DialogResult.No
        Close()
    End Sub
End Class