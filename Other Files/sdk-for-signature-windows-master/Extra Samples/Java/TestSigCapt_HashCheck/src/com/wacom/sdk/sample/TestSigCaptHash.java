/*
  TestSigCaptHash.java
  Captures a signature, generates a document hash and verifies it
  
  Copyright (c) 2019 Wacom. All rights reserved.
	
*/
package com.wacom.sdk.sample;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.image.BufferedImage;
import java.io.File;
import java.io.IOException;

import javax.imageio.ImageIO;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTextArea;

import com.florentis.signature.DynamicCapture;
import com.florentis.signature.SigCtl;
import com.florentis.signature.SigObj;
import com.florentis.signature.Hash;

public class TestSigCaptHash extends JFrame {
	
  private static final long serialVersionUID = 1L;
  
  private JPanel drawPanel;
  private JTextArea textArea;
  private BufferedImage signatureImage;
  
  public TestSigCaptHash() {
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.setTitle("TestSigCaptHash");
		this.setSize(new Dimension(450,350));
		this.setLayout(new BorderLayout());

		drawPanel = new JPanel() {
			@Override
			public void paintComponent(Graphics g) {
				super.paintComponent(g);
				if (signatureImage != null) {
					g.drawImage(signatureImage, 0, 0, null);
				}
			}
		};
		drawPanel.setBackground(Color.WHITE);
		drawPanel.setPreferredSize(new Dimension(200,150));
		

		JPanel panelImage = new JPanel();
		panelImage.add(drawPanel);
		
		this.add(panelImage,BorderLayout.WEST);
			
		JButton btnSign = new JButton("Sign");
		btnSign.setPreferredSize(new Dimension(100,50));
		btnSign.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent evt) {
				textArea.append("btnSign was pressed\n");
				sign();
			}
		});
		
		JPanel panelButton = new JPanel();
		panelButton.add(btnSign);
		
		this.add(panelButton, BorderLayout.EAST);
		
		textArea = new JTextArea(8,20);
		textArea.setEditable(false);
		this.add(new JScrollPane(textArea), BorderLayout.SOUTH);
		
  }

	private void sign() {
		SigCtl sigCtl = new SigCtl();
		sigCtl.licence("<<license>>");
		DynamicCapture dc = new DynamicCapture();
		
		// Generate hash value to attach to signature
		Hash hash = new Hash(); 
        GetHash(hash);
		
		int rc = dc.capture(sigCtl, "who", "why", hash, null);
		
        if(rc == 0) {
        	textArea.append("signature captured successfully\n");
			String fileName = "sig1.png";
			SigObj sig = sigCtl.signature();
			sig.extraData("AdditionalData", "CaptureImage.java Additional Data");
			int flags = SigObj.outputFilename | SigObj.color32BPP | SigObj.encodeData;
			sig.renderBitmap(fileName, 200, 150, "image/png", 0.5f, 0xff0000, 0xffffff, 0.0f, 0.0f, flags );
			paintSignature(fileName);
		}
		else {
			textArea.append("signature capture error res="+rc+"\n");
			switch(rc) {
				case 1:   textArea.append("Cancelled\n");
					break;
				case 100: textArea.append("Signature tablet not found\n");
					break;
				case 103: textArea.append("Capture not licensed\n");
					break;
				default:  textArea.append("Unexpected error code\n");
			}
		}
  }
	
	private void GetHash(Hash hash) {
        textArea.append("Creating document hash:\n");
        hash.clear();
        hash.type(Hash.md5);
        hash.add("John");
        hash.add("Smith");
        textArea.append("hash.add: John\n");
        textArea.append("hash.add: Smith\n");
    }
	  
	private void paintSignature(String fileName) {
		try
    {
	    signatureImage = ImageIO.read(new File(fileName));
	    drawPanel.repaint();
	    
    } catch (IOException e)
    {
    	System.out.println(e.toString());
    }
	}
	
	public static void main(String args[]) {
		TestSigCaptHash frame = new TestSigCaptHash();
		frame.setVisible(true);
	}
	
	 
	
}
