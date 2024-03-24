// Created by Aryan "Tony" Gupta on 17 March 2024.

/*
  Heavily references Michael Schoeffler's video on RFID-RC522: https://youtu.be/wgNRYOiNPtM?si=HyeYGWs-pl4h6qbK
  Also references Playful Technology's video on handling multiple RFID connections: https://youtu.be/f_f_5cL0Pd0?si=yVMC0mNreQ1Jag6x
*/

#include <SPI.h>

#include <MFRC522.h>
#include <MFRC522Extended.h>
#include <deprecated.h>
#include <require_cpp11.h>

// Global variables
const int numRFIDReaders = 5;
const int SDAPins[] = {40, 41, 42, 43, 45};
const int ResetPin = 38;
// Using the hardware ICSP pins, so reset shouldn't *need* to be defined...
// Initialize the readers in an array
MFRC522 mfrc522[numRFIDReaders];

void setup() {
  // put your setup code here, to run once:
  // Open the Serial Connection
  Serial.begin(9600);

  // Open the SPI Connection
  SPI.begin();

  // Initialize the chip on the RFID
  for(int i = 0; i < numRFIDReaders; i++)
  {
    mfrc522[i].PCD_Init(SDAPins[i], ResetPin);

    Serial.print(F("Reader: "));
    Serial.print(i);
    Serial.print(F(". On pin: "));
    Serial.print(String(SDAPins[i]));
    Serial.print(F(". Antenna strength: "));
    Serial.print(mfrc522[i].PCD_GetAntennaGain());
    Serial.print(F(". Version: "));
    mfrc522[i].PCD_DumpVersionToSerial();

    delay(100);
  }
  
}

void loop()
{
  // put your main code here, to run repeatedly:
  // Iterate through the readers in order
  for(int i = 0; i < numRFIDReaders; i++)
  {
    // Start up the scanner
    mfrc522[i].PCD_Init();

    String uidString = "";

    // Check for presence/proximity
    if(mfrc522[i].PICC_IsNewCardPresent())
    {
      // Check if a tag was read
      if(mfrc522[i].PICC_ReadCardSerial())
      {
        // Read the Unique ID of the RFID tag into uidString
        for (byte j = 0; j < mfrc522[i].uid.size; ++j)
        {
          if (mfrc522[i].uid.uidByte[j] < 0x10)
          {
            uidString += "0";
          }
          uidString += String(mfrc522[i].uid.uidByte[j], HEX);
        }

        // Serial.print(F("On Scanner "));
        // Serial.print(i);
        // Serial.print(", RFID Tag ID: ");
        // Serial.print(uidString);
        // Serial.println();
      }
    }
    // Empty out the array element if there is no block
    else
    {
      uidString = "EMPTY";
    }

    Serial.print(uidString);
    Serial.print(", ");
  }
  Serial.println();
}
