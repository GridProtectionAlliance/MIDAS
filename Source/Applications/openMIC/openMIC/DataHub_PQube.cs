﻿//******************************************************************************************************
//  DataHub_PQube.cs - Gbtc
//
//  Copyright © 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/06/2021 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using GSF.Units;

namespace openMIC
{
    public partial class DataHub
    {
        public string PQubeLoadConfig(int deviceID) =>
            // TODO: Load actual device config (FTP?)
            GetIniAsJson(PQubeGetSampleIni(), false);

        public dynamic PQubeGetDeviceTimeWithError(int deviceID)
        {
            // TODO: Query device for time
            DateTime parsedTime = DateTime.UtcNow;
            double totalError = (parsedTime - DateTime.UtcNow).TotalSeconds + 2.0D;
            return new
            {
                time = parsedTime.ToString(ShortDateTimeFormat),
                error = totalError,
                errorText = Time.ToElapsedTimeString(Math.Abs(totalError), 2)
            };
        }

        public string PQubeGetSampleIni()
        {
            return 
              @"; ------ PQube 3 from Powerside.
                ; ------ www.powerside.com
                ; ------ PQube 3e Version 3.9

                ;----------------------------------------------------
                [PQube_Information]
                ;----------------------------------------------------
                ; ------ Assign a unique identifier for your PQube 3
                PQube_ID=""Test""

                ; ------ Describe the place where your PQube 3 is installed
                Location_Name=""(location not set)""

                ; ------ Optional additional information about your PQube 3
                Note_1=""(note not set)""
                Note_2=""(note not set)""

                ; ------ Valid Values: AUTO, Single_Phase_L1_N, Single_Phase_L1_L2, Star, Wye, Delta, Split_Phase, NO_MAINS
                ; ------ AUTO sets the power configuration depending on the voltages that are applied to the PQube 3 AC mains terminals during startup.
                ; ------ Choose NO_MAINS to enable data recording using only the auxiliary channels (analog, digital, environmental). No mains AC voltage is required to begin recording.
                Power_Configuration=AUTO

                ; ------ Label of the Time zone region where your PQube 3 is located (America/Los_Angeles, Europe/Paris, etc..)
                Time_Zone_Region=America/Los_Angeles

                ; ------ Duration in minutes of battery back up before your PQube 3 automatically shuts down
                ; ------ This applies only if your PQube has a UPS module
                ; ------ Valid values: 3 to 30, typical value 5
                UPS_Time_In_Minutes=5

                ; ------ UPS module model connected to PQube3.
                ; ------ Valid values: None, UPS1, UPS2, UPS3 default is None
                UPS_Model=None

                ; ------ Capacity of the battery pack connected to the UPS2 module. If there are several battery packs connected,
                ; ------ the capacity is the total capacity for all packs together
                ; ------ Note: This parameter is ignored if a UPS1 module is connected to the PQube3.
                ; ------ Valid values: 2500 to 7500, default is 2500
                UPS_Battery_Capacity_In_mAh=

                ; ------ Selecting UPS means that the Power module AUX output is ON when UPS is present and prime power is present.
                ; ------ If UPS is not present or prime power is not present then Power module AUX output is OFF.
                ; ------ Valid values: ON, OFF, UPS, default is UPS
                PM_AUX_Output_Cntl=UPS

                ; ------ Your PQube 3's internal fan turns on when the CPU temperature exceeds this threshold.
                ; ------ Valid values: integer between 40 and 60, typical value 45
                Fan_Temperature_Threshold_in_DegC=45

                ; ------ Choose the language for web pages, screen display, and graphs generated by your PQube 3.
                PQube_Primary_Language=English-American
                PQube_Secondary_Language=None

                ; ------ Attenuator model used (I1,I2,I3 terminals) along with PQube3 dual voltage mode
                ; ------ Note: This parameter is ignored by the PQube3 firmware, but used by the configurator
                ; ------ Valid values: VAT1-600, None  default is None
                Attenuator_Model_DualVoltageMode=None

                ; ------ Bluetooth interface PQube3.
                ; ------ Valid values: OFF, ON. Default is ON
                Enable_Bluetooth=ON

                ;----------------------------------------------------
                [Data_Backup]
                ;----------------------------------------------------
                ; ------ If enabled, your PQube 3 will perform a measurement data backup from its memory
                ; ------ to either the extractible microSDCard or to the USB thumb drive.
                ; ------ the copy occurs once a day, if the media is present in its slot.
                ; ------ Valid Values: OFF, ON. Default is ON
                Enable_Data_Backup=ON

                ; ------ Valid Values: USB, SDCARD
                Data_Backup_to=SDCARD

                ;----------------------------------------------------
                [Nominal_Inputs]
                ;----------------------------------------------------
                ; ------ Choose the nominal value of the mains voltage measured in volts, taking into account transformer ratios if applicable
                ; ------ Valid Values: AUTO, positive value between 69 and 800000
                ; ------ AUTO sets the nominal voltage using the actual voltages at the mains AC terminals during startup and rounds to the nearest standard worldwide voltage.
                ; ------ Examples of values when using transformer ratios: 11000, 12470, 33000

                ; ------ Typical values for Phase to Phase voltage are 208, 380, 400, 480
                Nominal_Phase_To_Phase_Voltage=AUTO

                ; ------ Typical values for Phase to Neutral voltage are 100, 120, 230, 277
                Nominal_Phase_To_Neutral_Voltage=AUTO

                ; ------ Valid Values: AUTO, 16.67, 50, 60, 400
                Nominal_Frequency=AUTO

                ;----------------------------------------------------
                [Channel_Recordings]
                ;----------------------------------------------------
                ; ------ Choose the types of files your PQube will generate for events and trends
                ; ------ Valid Values: ON, OFF
                Generate_GIF_Graphs=ON
                Generate_PQDIF_Files=OFF

                ; ------ Choose the number of samples/cycle for waveform recordings
                ; ------ This does not change your PQube 3's sampling rate, only the level of detail in your waveform graphs. Your PQube 3 always samples at 512 samples/cycle.
                ; ------ Your choice is a tradeoff between graph resolution and the overall number of cycles displayed in the graph.
                ; ------ Valid Values: 32, 64, 128, 256, 512.  Typical value 256
                ; ------ There are 4096 total samples in the waveform buffer. Your PQube records 8 cycles at value 512, 16 cycles at value 256, ..., 128 cycles at value 32
                Recorded_Samples_Per_Cycle=256

                ; ------ Valid Values: Urms1/2, Urms1
                ; ------ Urms1/2 is the single-cycle RMS window sliding each half cycle as defined in IEC 61000-4-30.
                ; ------ Urms1 is the non-overlapping single-cycle RMS (sliding each cycle).
                ; ------ Urms1  produces half as much data per second, so the duration of the RMS recording is doubled.
                Event_RMS_Recording_Definition=""Urms1/2""

                ; ------ Your PQube normally records the beginning and end of an event. Use this tag to use all of the recodring space to capture the beginning of the event.
                ; ------ normally used to capture the end of an event to be used to extend the capture of the
                ; ------ beginning of the event.
                ; ------ Valid Values: ON, OFF
                Capture_End_Of_Event=ON

                ; ------ Valid Values: ON, OFF, AUTO
                ; ------ AUTO sets the channel recording to ON based on power configuration.
                Record_Phase_To_Phase_Channels=AUTO
                Record_Phase_To_Neutral_Channels=AUTO
                Record_Current_I1_I2_I3_Channel=AUTO
                Record_Current_I4_Channel=AUTO
                Record_Current_I5_Channel=AUTO
                ; ------ Valid Values: ON, OFF
                Record_Current_I6_Channel=OFF
                Record_Current_I7_Channel=OFF
                Record_Current_I8_Channel=OFF

                ; ------ Channels 9 through 14 are available if your PQube3 comes with add-on 6 current channel board.
                ; ------ This is a factory configuration and cannot be changed in the field
                ; ------ The PQube 3 model with 14 channel is PQUBE3-PQ-E08N-E06N-XXXX
                Record_Current_I9_Channel=OFF
                Record_Current_I10_Channel=OFF
                Record_Current_I11_Channel=OFF
                Record_Current_I12_Channel=OFF
                Record_Current_I13_Channel=OFF
                Record_Current_I14_Channel=OFF

                ; ------ Valid Values: ON, OFF, AUTO
                ; ------ AUTO sets the channel recording to ON if events are enabled for that channel.
                Record_AN1_E_Channel=AUTO
                Record_AN2_E_Channel=AUTO
                Record_AN1_AN2_Differential_Channel=AUTO
                Record_AN3_E_Channel=AUTO
                Record_AN4_E_Channel=AUTO
                Record_AN3_AN4_Differential_Channel=AUTO
                Record_DIG1_Channel=AUTO

                ; ------ Valid Values: ON, OFF
                Record_Flicker=ON
                Record_Voltage_THD=ON
                Record_Current_TDD=ON
                Record_Voltage_Unbalance=ON
                Record_Current_Unbalance=ON

                ; ------ Enabling this will have PQube 3 trend time be in UTC
                ; ------ Valid Values: ON, OFF, default OFF
                Enable_PQVIEW_Compatibility_Mode=OFF
                Enable_1_Min_PQDIF_Indexing=OFF

                ; ------ Enabling this will extend the event RMS recording to 30sec, with 1 sec pretrigger approx.
                ; ------ Waveform recording will remain unchanged, and RMS recording will contain L-N/L-L, phase and neutral currents, and frequency.
                ; ------ Valid Values: ON, OFF, default OFF
                Enable_Extended_RMS_Event_Recording=OFF

                ;----------------------------------------------------
                [Measurement_Setup]
                ;----------------------------------------------------
                ; ------ Define peak demand interval - peak watts, VA, amps - 1 cycle and min are already included
                ; ------ Valid Values: 3, 5, 10, 15, 20, 30, 60, typical values 10 mins (or 15 in North America)
                Peak_Demand_Interval_In_Minutes=15

                ; ------ Power factor is tPF when BUDEANU is selected
                ; ------ Power factor is dPF when FUNDAMENTAL is selected
                ;------- Valid values: BUDEANU, FUNDAMENTAL
                VAR_Calculations=FUNDAMENTAL

                ; ------ Energy output KYZ, expressed in Watt-hour per pulse.
                ; ------ Valid Values: 0 for disabled output, between X and Y
                KYZ_Relay_in_Wh_per_Pulse=0

                ; ------ Valid range 0-17, where 0, 6, 10, 14 correspond to the energy groups.
                ; ------ 0 is the sum of 1,2,3 energies ( the ""Enable_3Phase_Group_on_1_2_3"" )
                ; ------ 4 and 5 are individual ( Energies associated with ""Current_I4_associated_to_Voltage"" and ""Current_I5_associated_to_Voltage"" )
                ; ------ 6 is the sum of 7,8,9 ( the ""Name_of_3Phase_Load_on_6_7_8"" ) etc.
                KYZ_Relay_Wh_Channel_Select=0

                ; ------ Valid values are OFF, ON.
                Record_IEC_61000-4-30_10_Min_Interval=ON

                ; ------ Record frequency using 10 second interval to a CSV file (IEC 61000-4-30 Class A).
                ; ------ Valid values are OFF and ON.
                Record_10_Second_Frequency=ON

                ; ------ Valid values are OFF and ON. Choose ON to record the daily 2-150kHz trend file
                Record_2-150kHz_Conducted_Emissions=ON

                ; ------ Set the full-scale value of your current measurement channels. Valid values are HIGH (±10Vpk full scale) or LOW (0.333Vrms nominal)
                ; ------ Valid values are HIGH, LOW, INDIVIDUAL_BY_CHANNEL, typical value LOW
                Current_Range=""LOW""
                Secondary_Current_Range=""LOW""

                Current_I1_Range=LOW
                Current_I4_Range=LOW
                Current_I5_Range=LOW
                Current_I6_Range=LOW
                Current_I7_Range=LOW
                Current_I8_Range=LOW
                Current_I9_Range=LOW
                Current_I10_Range=LOW
                Current_I11_Range=LOW
                Current_I12_Range=LOW
                Current_I13_Range=LOW
                Current_I14_Range=LOW

                ; ------ Choose the rated current used for TDD calculation. See IEEE 519.
                ; ------ typical value for AUTO is 5
                TDD_Available_Current_In_Amps=AUTO

                ; ------ Set the lamp voltage according to IEC 61000-4-15
                ; ------ Valid Values: 120, 230
                Flicker_Lamp_Voltage=120

                ; ------ Valid values: ANSI, IEC, GB, IEEE_112, IEEE_936, default IEC.
                Unbalance_Calculation_Method=IEC

                ; ------ The mode of recording Voltage Harmonics, in volts or in % of Fundamental
                ; ------ Also determines whether the THD or TDD will be calculated
                ; ------ Valid values: ""Volts_and_THD"", ""Percent_of_Fundamental_and_THD""
                Voltage_Harmonics_Unit=Volts_and_THD

                ; ------ Current Harmonics are always recorded in amps
                ; ------ This tag determines the definition of current distortion: TDD  or THDi
                ; ------ Valid values: ""Current_Distortion_TDD"", ""Current_Distortion_THDi""
                Current_Harmonics_Distortion=Current_Distortion_TDD

                ; ------ Default individual is computed in Amps, this is the option the compute the harmonics in percent of IL
                ; ------ this tag today only applies to the 10min file
                ; ------ default is OFF
                Individual_Harmonics_in_Percent_of_IL=OFF

                ; ------ Valid values: ON/OFF, default is OFF
                Record_Energy_Metering=OFF

                ; ------ Valid values: 3, 5, 10, 15, 30, default is 15
                Energy_Metering_Interval_in_Minutes=15

                ; ------ The rotational method of the phase vectors on the phasor screen
                ; ------ Valid values: CLOCKWISE, COUNTER_CLOCKWISE
                Rotation_Convention_on_Vectors_Screen=COUNTER_CLOCKWISE

                ; ------ PQube3 will record summary counter events, after exceeding quota of events per hour and a quota per day.
                ; ------ the quota is 15 events per hour, and 200 events per day, or 50 events of the same type per day
                ; ------ Valid values: OFF, ON, default OFF
                Enable_Event_Overflow_Protection=OFF

                ;----------------------------------------------------
                [Energy_Metering_Setup]
                ;----------------------------------------------------
                ; ------ The following tags association between current channels and
                ; ------ voltage channels for power computation
                ; ------ If the tag is None, there will be no power computed or recorded
                ; ------ for the curent channel load
                ; ------ Valid values: L1, L2, L3, None
                Current_I1_associated_to_Voltage=L1
                Current_I2_associated_to_Voltage=L2
                Current_I3_associated_to_Voltage=L3
                Current_I4_associated_to_Voltage=None
                Current_I5_associated_to_Voltage=None
                Current_I6_associated_to_Voltage=None
                Current_I7_associated_to_Voltage=None
                Current_I8_associated_to_Voltage=None
                Current_I9_associated_to_Voltage=None
                Current_I10_associated_to_Voltage=None
                Current_I11_associated_to_Voltage=None
                Current_I12_associated_to_Voltage=None
                Current_I13_associated_to_Voltage=None
                Current_I14_associated_to_Voltage=None

                ; ------ Name of the load 5 characters maximum (the name is truncated to 5 characters)
                Name_of_1Phase_Load_1=
                Name_of_1Phase_Load_2=
                Name_of_1Phase_Load_3=
                Name_of_1Phase_Load_4=
                Name_of_1Phase_Load_5=
                Name_of_1Phase_Load_6=
                Name_of_1Phase_Load_7=
                Name_of_1Phase_Load_8=
                Name_of_1Phase_Load_9=
                Name_of_1Phase_Load_10=
                Name_of_1Phase_Load_11=
                Name_of_1Phase_Load_12=
                Name_of_1Phase_Load_13=
                Name_of_1Phase_Load_14=

                ; ------ This tag allow to declare split phase or 3 phase loads associated
                ; ------ with current channels I1,I2,I3
                ; ------ Valid values: ON/OFF, default is ON
                Enable_3Phase_Group_on_1_2_3=ON
                Name_of_3Phase_Load_on_1_2_3=Mains

                ; ------ This tag allow to declare split phase or 3 phase loads associated
                ; ------ with current channels I6,I7,I8
                ; ------ Valid values: ON/OFF, default is OFF
                Enable_3Phase_Group_on_6_7_8=OFF
                Name_of_3Phase_Load_on_6_7_8=

                ; ------ This tag allow to declare split phase or 3 phase loads associated
                ; ------ with current channels I9,I10,I11
                ; ------ Valid values: ON/OFF, default is OFF
                Enable_3Phase_Group_on_9_10_11=OFF
                Name_of_3Phase_Load_on_9_10_11=

                ; ------ This tag allow to declare split phase or 3 phase loads associated
                ; ------ with current channels I12,I13,I14
                ; ------ Valid values: ON/OFF, default is OFF
                Enable_3Phase_Group_on_12_13_14=OFF
                Name_of_3Phase_Load_on_12_13_14=

                ;----------------------------------------------------
                [Dual_Voltage_Measurement]
                ;----------------------------------------------------
                ; ------ Enables (ON) the dual voltage measurement
                ; ------ Valid values: ON/OFF default is OFF
                Dual_Voltage_Mode=OFF

                ; ------ Enables (ON) the dual power measurement
                ; ------  This mode is supported only in a PQube 3e
                ; ------ Valid values: ON/OFF default is OFF
                Dual_Power_Mode=OFF

                ; ------ Nominal value of 2nd set of voltage
                ; ------ the value must be expressed in primary voltage
                ; ------ (i.e. multiplied by the ratio used in the Current_Transformer_Ratio tag)
                Voltage_2_Nominal_Voltage=120

                ; ------ wiring/cabling type for the 2nd set of voltage,
                ; ------ will control the downstream phase display
                ; ------ Valid values: Wye, Delta, Single_Phase_L1_N, or 2_Differential_Channels, default is Wye
                Voltage_2_Cabling_Configuration=Wye

                ; ------ Label for mains voltage
                ; ------ maximum 64 characters, this identifies the mains input voltage in meters and graphs
                ; ------ default ""Upstream voltage""
                Voltage_Mains_Label=""Upstream voltage""

                ; ------ Label for 2nd set of voltage
                ; ------ maximum 64 characters, this identifies the 2nd set of voltage in meters and graphs
                ; ------ default ""Downstream voltage""
                Voltage_2_Label=""Downstream voltage""
                Voltage_2_L1_Dip_Events=OFF
                Voltage_2_L1_Swell_Events=OFF
                Voltage_2_L1_Dip_Thresholds_In_Percent_of_Voltage_2_Nominal=90
                Voltage_2_L1_Swell_Thresholds_In_Percent_of_Voltage_2_Nominal=110
                Voltage_2_L1_Hysteresis_Thresholds_In_Percent_of_Voltage_2_Nominal=2
                Voltage_2_L2_Dip_Events=OFF
                Voltage_2_L2_Swell_Events=OFF
                Voltage_2_L2_Dip_Thresholds_In_Percent_of_Voltage_2_Nominal=90
                Voltage_2_L2_Swell_Thresholds_In_Percent_of_Voltage_2_Nominal=110
                Voltage_2_L2_Hysteresis_Thresholds_In_Percent_of_Voltage_2_Nominal=2
                Voltage_2_L3_Dip_Events=OFF
                Voltage_2_L3_Swell_Events=OFF
                Voltage_2_L3_Dip_Thresholds_In_Percent_of_Voltage_2_Nominal=90
                Voltage_2_L3_Swell_Thresholds_In_Percent_of_Voltage_2_Nominal=110
                Voltage_2_L3_Hysteresis_Thresholds_In_Percent_of_Voltage_2_Nominal=2

                ;----------------------------------------------------
                [Potential_Transformers]
                ;----------------------------------------------------
                ; ------ Valid values: from 1:1 up to 50000:1  1:1 no tfo
                ; ------ You can use fractional values such as 1250.5:120
                Potential_Transformer_Ratio=1:1

                ;----------------------------------------------------
                [Current_Transformers]
                ;----------------------------------------------------
                ; ------ Valid values: CT, ROGOWSKI_CT, Default is CT
                Current_I1_Transformer_Type=CT
                Current_I2_Transformer_Type=CT
                Current_I3_Transformer_Type=CT
                Current_I4_Transformer_Type=CT
                Current_I5_Transformer_Type=CT
                Current_I6_Transformer_Type=CT
                Current_I7_Transformer_Type=CT
                Current_I8_Transformer_Type=CT
                Current_I9_Transformer_Type=CT
                Current_I10_Transformer_Type=CT
                Current_I11_Transformer_Type=CT
                Current_I12_Transformer_Type=CT
                Current_I13_Transformer_Type=CT
                Current_I14_Transformer_Type=CT

                ; ------ Valid values: from 1:1 up to 10000:1
                ; ------ For voltage-input current modules, the second number is the voltage (typically 0.333).
                ; ------ You can use fractional values such as 100.3:0.333 or 5:0.333
                Current_Transformer_Ratio=1:1
                Neutral_Current_Transformer_Ratio=1:1
                Earth_Current_Transformer_Ratio=1:1
                Current_I6_Transformer_Ratio=1:1
                Current_I7_Transformer_Ratio=1:1
                Current_I8_Transformer_Ratio=1:1
                Current_I9_Transformer_Ratio=1:1
                Current_I10_Transformer_Ratio=1:1
                Current_I11_Transformer_Ratio=1:1
                Current_I12_Transformer_Ratio=1:1
                Current_I13_Transformer_Ratio=1:1
                Current_I14_Transformer_Ratio=1:1

                Current_I1_calibration_SN=
                Current_I2_calibration_SN=
                Current_I3_calibration_SN=
                Current_I4_calibration_SN=
                Current_I5_calibration_SN=
                Current_I6_calibration_SN=
                Current_I7_calibration_SN=
                Current_I8_calibration_SN=
                Current_I9_calibration_SN=
                Current_I10_calibration_SN=
                Current_I11_calibration_SN=
                Current_I12_calibration_SN=
                Current_I13_calibration_SN=
                Current_I14_calibration_SN=

                ; ------ Valid values: OFF, L1, L2, L3, N, E
                ; ------ If you have current transformers on all conductors but one, your
                ; ------ PQube3 can calculate the remaining channel.
                ; ------ L1: =-(I2+I3), L2: =-(I1+I3), L3: =-(I1+I2)
                ; ------ N : = (I1+I2+I3) Neutral is the sum of phase current channels.
                ; ------ E : = (I1+I2+I3+I4) - Earth current is the sum of all phase and neutral current
                Calculate_Remaining_Current_Channel=OFF

                I1_Terminal_Channel_Label=""L1""
                I2_Terminal_Channel_Label=""L2""
                I3_Terminal_Channel_Label=""L3""
                I6_Terminal_Channel_Label=""L6""
                I7_Terminal_Channel_Label=""L7""
                I8_Terminal_Channel_Label=""L8""
                I9_Terminal_Channel_Label=""L9""
                I10_Terminal_Channel_Label=""L10""
                I11_Terminal_Channel_Label=""L11""
                I12_Terminal_Channel_Label=""L12""
                I13_Terminal_Channel_Label=""L13""
                I14_Terminal_Channel_Label=""L14""

                ;----------------------------------------------------
                [Adjust_Phase_Connections]
                ;----------------------------------------------------
                ; ------ Use this section to correct any connection errors made during installation.
                ; ------ (Voltage_L1_Input_Connected_To refers to the L1 voltage terminal on your PQube 3.)
                ; ------ (Current_I1_Amps_Input_Connected_To refers to the I1 +/- terminals on your PQube 3.)
                ; ------ Valid values: L1, L2, L3, N, and E
                Voltage_L1_Input_Connected_To=L1
                Voltage_L2_Input_Connected_To=L2
                Voltage_L3_Input_Connected_To=L3
                Voltage_N_Input_Connected_To=N

                ; ------ Valid Values: I1, I2, I3, I4, I5, I6, I7, I8, I9, I10, I11, I12, I13, I14.
                ; ------ I1_Input_Connected_To refers to the I1 position on your PQube3 current input terminal.
                Current_I1_Input_Connected_To=I1
                Current_I2_Input_Connected_To=I2
                Current_I3_Input_Connected_To=I3
                Current_I4_Input_Connected_To=I4
                Current_I5_Input_Connected_To=I5
                Current_I6_Input_Connected_To=I6
                Current_I7_Input_Connected_To=I7
                Current_I8_Input_Connected_To=I8
                Current_I9_Input_Connected_To=I9
                Current_I10_Input_Connected_To=I10
                Current_I11_Input_Connected_To=I11
                Current_I12_Input_Connected_To=I12
                Current_I13_Input_Connected_To=I13
                Current_I14_Input_Connected_To=I14

                ; ------ Valid Values: ON, OFF
                Invert_Current_I1_Channel=OFF
                Invert_Current_I2_Channel=OFF
                Invert_Current_I3_Channel=OFF
                Invert_Current_I4_Channel=OFF
                Invert_Current_I5_Channel=OFF
                Invert_Current_I6_Channel=OFF
                Invert_Current_I7_Channel=OFF
                Invert_Current_I8_Channel=OFF
                Invert_Current_I9_Channel=OFF
                Invert_Current_I10_Channel=OFF
                Invert_Current_I11_Channel=OFF
                Invert_Current_I12_Channel=OFF
                Invert_Current_I13_Channel=OFF
                Invert_Current_I14_Channel=OFF

                ;----------------------------------------------------
                [Analog_Channel_Setup]
                ;----------------------------------------------------
                ; ------ Toggle Analog Energy mode. AN1-AN2 differential channel becomes Analog Power (AN1xAN2) and Analog Energy (AN1xAN2xHours)
                ; ------ Apply voltage to AN1 and current to AN2.
                ; ------ Apply voltage to AN3 and current to AN4.
                ; ------ Valid Values: ON OFF
                AN1xAN2_Energy_Mode=OFF
                AN3xAN4_Energy_Mode=OFF

                ; ------ Valid values: from 1:1 up to 10000:1. You can use fractional values.
                AN1_E_Channel_Ratio=1:1
                AN2_E_Channel_Ratio=1:1
                AN3_E_Channel_Ratio=1:1
                AN4_E_Channel_Ratio=1:1

                ; ------ Range for ANx_E input channels : valid values are HIGH (+/-100V Full scale) or LOW (+/-10V Full scale)
                AN1_E_Range=""HIGH""
                AN2_E_Range=""HIGH""
                AN3_E_Range=""HIGH""
                AN4_E_Range=""HIGH""

                ; ------ If you are measuring an AC signal on the analog channels, set the measurement mode to AC to compute RMS (positive values only).
                ; ------ If you are measuring a DC signal on the analog channels, set the measurement mode to DC to compute average (positive and negative values).
                ; ------ If you are measuring a DC signal on the analog channels with an ATT1 module, choose the model (ATT1_0600V, ATT1_1200V, ATT1_2400V, ATT1_4800V).
                ; ------ Valid Values: AC, DC, DC_ATT1_0600V, DC_ATT1_1200V, DC_ATT1_2400V, DC_ATT1_4800V
                AN1_and_AN2_Measurement_Mode=DC
                AN3_and_AN4_Measurement_Mode=DC

                ; ------ Use this section of tags to customize the names and units of your analog channels.
                ; ------ Analog energy mode does not have to be on for this.
                ; ------ Valid names can be up to 5 characters.
                ; ------ Valid unit values are ""V"", ""A"", ""W"", ""DEG"", ""%""
                AN1_E_Channel_Name=""AN1-E""
                AN1_E_Channel_Unit=""V""
                AN2_E_Channel_Name=""AN2-E""
                AN2_E_Channel_Unit=""V""
                AN1_AN2_Channel_Name=""AN1-AN2""

                AN3_E_Channel_Name=""AN3-E""
                AN3_E_Channel_Unit=""V""
                AN4_E_Channel_Name=""AN4-E""
                AN4_E_Channel_Unit=""V""
                AN3_AN4_Channel_Name=""AN3-AN4""

                AN1_E_Channel_Offset=0
                AN2_E_Channel_Offset=0
                AN3_E_Channel_Offset=0
                AN4_E_Channel_Offset=0

                ;----------------------------------------------------
                [EnviroSensor_Probe_Setup]
                ;----------------------------------------------------
                ; ------ The EnviroSensor Probe serial number format is ""E"" followed by 7 digits (e.g. ""E3001163""), default is blank
                ; ------ Valid Channel Names can be up to 7 characters, default is blank
                ; ------ If the following tags are left blank:
                ; ------ 1 probe connected, it becomes Probe_A.
                ; ------ 2 probes connected, Probe_A is the lowest serial number,
                ; ------ Probe_B is the highest serial number
                Probe_A_Serial_Number=
                Probe_A_Channel_Name=""""
                Probe_B_Serial_Number=
                Probe_B_Channel_Name=""""

                ; ------ ENV2 Acceleration settings
                ; ------ the tags below do not apply for ENV1 EnviroSensors
                ; ------ Valid Values: ON, OFF, Default is ON
                ; ------ flashing LED may attract unwanted attention, use OFF to have all LEDs turned off
                ; ------ When value is ON the Probe LED will:
                ; ------ GREEN blinking slow - normal operation
                ; ------ GREEN blinking fast - event detection
                ; ------ RED steady - transmitting recording to PQube 3
                ; ------ RED blinking fast - Error
                Probe_A_LED_Activity=ON
                Probe_B_LED_Activity=ON

                ; ------ Valid Values: 2, 4, 8. Default is 2
                ; ------ Suggested values:
                ; ------ 2 for tilt detection, earthquakes, and other precision applications
                ; ------ 4 for gentle road bumps, careful loading/unloading
                ; ------ 8 for violent bumps, rough loading/unloading, other coarse applications
                Probe_A_Acceleration_Range_in_g=2
                Probe_B_Acceleration_Range_in_g=2

                ; ------ Earth's gravity vector is a constant 1g acceleration.
                ; ------ For differential applications where signals of frequency greater than 2 Hz are of interest, it can be a non-information bearing offset
                ; ------ However, the gravity vector is necessary for Tilt applications
                ; ------ so leave it OFF for Tilt.
                ; ------ ON implies a 2 Hz high-pass filter applied to samples
                ; ------ Valid values: ON, OFF. Default is ON.
                Probe_A_Remove_Gravity_Vector=ON
                Probe_B_Remove_Gravity_Vector=ON

                ; ------ Recording number of samples per second
                ; ------ 32 samples per second = 15 seconds recorded for each event
                ; ------ 16 samples per second = 30 seconds recorded for each event
                ; ------ 8 samples per second = 60 seconds recorded for each event
                ; ------ Valid Values:  32, 16, or 8. Default is 32
                Probe_A_Acceleration_Samples_Per_Second=32
                Probe_B_Acceleration_Samples_Per_Second=32

                ; ------ The unit in which the acceleration is recorded and displayed in graphs, meters, files, etc.
                ; ------ NOTE: This does not change the thresholds in this file,
                ; ------ which are all in ""g"".  It only affects the records and displays.
                ; ------ Valid Values: ""g"", ""METERS_PER_SECOND_SQUARED"". Default is ""g""
                Probe_A_Acceleration_Display_Unit=""g""
                Probe_B_Acceleration_Display_Unit=""g""

                ; ------ ENV2 Probe Shock Events
                ; ------ Valid Values:  ON, OFF. Default is OFF
                Enable_Probe_A_Mechanical_Shock_Events=OFF
                Enable_Probe_B_Mechanical_Shock_Events=OFF

                ; ------ Select the Shock/vibration detection threshold
                ; ------ Valid values:  0.003 (most sensitive) to 0.4 (least sensitive). Default 0.05*20/.063
                Probe_A_Mechanical_Shock_Threshold_in_g_per_millisecond=0.050
                Probe_B_Mechanical_Shock_Threshold_in_g_per_millisecond=0.050

                ; ------ Detects short term orientation changes (ON) or Tilt deviation from a
                ; ------ continuously derived reference orientation
                ; ------ If set to OFF, reference orientation is derived at startup
                ; ------ Valid Values: ON, OFF. Default is OFF
                Probe_A_Tilt_Orientation_Change_Mode=OFF
                Probe_B_Tilt_Orientation_Change_Mode=OFF

                ; ------ ENV2 Probe Tilt Events
                ; ------ Enables or disable the tilt detection
                ; ------ Valid values: ON, OFF, default is OFF
                Enable_Probe_A_Tilt_Events=OFF
                Enable_Probe_B_Tilt_Events=OFF

                ; ------ Tilt Event Threshold
                ; ------ Valid Values: 0 to 180 (degrees)
                Probe_A_Tilt_Threshold_in_Degrees=90
                Probe_B_Tilt_Threshold_in_Degrees=90

                ; ------ ENV2 Probe Seismic Events
                ; ------ Valid Values: ON, OFF. Default is OFF
                ; ------ Seismic events are optimized for detecting even tiny accelerations
                ; ------ in the Seismic frequency range - from 0.1 Hz to 10 Hz.
                Enable_Probe_A_Seismic_Events=OFF
                Enable_Probe_B_Seismic_Events=OFF

                ; ------ Select the Seismic detection threshold
                ; ------ Valid values:  0.001 to 1.24, default is 0.02
                ; ------ 0.001  - Earthquake USGS Category 1.0 (not felt) and above
                ; ------ 0.0017 - Earthquake USGS Category 2.0-3.0 (weak) and above
                ; ------ 0.014  - Earthquake USGS Category 4.0 (light) and above
                ; ------ 0.039  - Earthquake USGS Category 5.0 (moderate) and above
                ; ------ 0.092  - Earthquake USGS Category 6.0 (strong) and above
                ; ------ 0.180  - Earthquake USGS Category 7.0 (very strong) and above
                ; ------ 0.340  - Earthquake USGS Category 8.0 (severe) and above
                ; ------ 0.650  - Earthquake USGS Category 9.0 (violent) and above
                ; ------ 1.240  - Earthquake USGS Category 10.0 (extreme) and above
                Probe_A_Seismic_Threshold_in_g=0.0200
                Probe_B_Seismic_Threshold_in_g=0.0200

                ; ------ ENV2 Probe Vibration Events
                ; ------ Valid Values: ON, OFF. Default is OFF
                ; ------ Vibration events are triggered when there is a change of state of the vibration. This change of state can be:
                ; ------ Vibration ON (detection of a vibration form an idle/steady state)
                ; ------ Vibration OFF (detection of the return to an idle/steady state)
                ; ------ the vibration is defined as a sustained variation of the acceleration for a minimum duration
                ; ------ above an acceleration threshold value.
                Enable_Probe_A_Vibration_Events=OFF
                Enable_Probe_B_Vibration_Events=OFF

                ; ------ Select the vibration detection threshold in g
                ; ------ Valid values:  0.002g to 2g default is 0.010g
                Probe_A_Vibration_Threshold_in_g=0.010
                Probe_B_Vibration_Threshold_in_g=0.010

                ; ------ Select the minimum vibration duration to trigger an event (in seconds)
                ; ------ Valid values:  1.0s to 5.0s,  default is 2.0s
                Probe_A_Vibration_minimum_duration_in_Seconds=2
                Probe_B_Vibration_minimum_duration_in_Seconds=2

                ; ------ Select the vibration normal/default  state
                ; ------ Valid values:  ON (mechanical vibration is the default state), OFF (mechanical idle/steady state is default state)
                ; ------ default value OFF
                Probe_A_Vibration_Normal_Condition=OFF
                Probe_B_Vibration_Normal_Condition=OFF

                ;----------------------------------------------------
                [Phase_To_Neutral_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF, AUTO
                Phase_To_Neutral_Events=AUTO
                Phase_To_Neutral_Dip_Threshold_In_Percent=90.00
                Phase_To_Neutral_Swell_Threshold_In_Percent=110.00
                Phase_To_Neutral_Interruption_Threshold_In_Percent=10.00
                Phase_To_Neutral_Event_Hysteresis_In_Percent=2.00

                ;----------------------------------------------------
                [Phase_To_Phase_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF, AUTO
                Phase_To_Phase_Events=AUTO
                Phase_To_Phase_Dip_Threshold_In_Percent=90.00
                Phase_To_Phase_Swell_Threshold_In_Percent=110.00
                Phase_To_Phase_Interruption_Threshold_In_Percent=10.00
                Phase_To_Phase_Event_Hysteresis_In_Percent=2.00

                ;----------------------------------------------------
                [Phase_To_Neutral_RVC_Events]
                ;----------------------------------------------------
                ; ------ RVC or Rapid Voltage Change is an abrupt change in voltage magnitude between two steady states of the voltage
                ; ------ within the limits of a swell and sag threshold limits.
                ; ------ An RVC event is characterized by its duration, the maximum voltage magnitude deviation during the event,
                ; ------ and by the variation before and after the event
                ; ------ Valid Values: ON, OFF, AUTO
                Phase_To_Neutral_RVC_Events=OFF

                ; ------ Minimum variation magnitude threshold to trigger the event (expressed in %Udin)
                ; ------ Threshold values should be lower the sag/swell variation thresholds.
                ; ------ For example RVC threshold should be < 10% Udin if the sag threshold is 90% Udin remaining voltage
                ; ------ Valid values 0.01 to 99
                Phase_To_Neutral_RVC_Threshold_In_Percent=6
                Phase_To_Neutral_RVC_Hysteresis_In_Percent=2

                ;----------------------------------------------------
                [Phase_To_Phase_RVC_Events]
                ;----------------------------------------------------
                ; ------ A phase to phase RVC or Rapid Voltage Change is an abrupt change in voltage magnitude
                ; ------ between two steady states of the voltage evaluated on the Phase to Phase voltage channels.
                ; ------ An RVC event is characterized by its duration, the maximum voltage magnitude deviation during the event,
                ; ------ and by the variation before and after the event
                ; ------ Valid Values: ON, OFF, AUTO
                Phase_To_Phase_RVC_Events=OFF

                ; ------ Minimum variation magnitude threshold to trigger the event (expressed in %Udin)
                ; ------ Threshold values should be lower the sag/swell variation thresholds.
                ; ------ For example RVC threshold should be < 10% Udin if the sag threshold is 90% Udin remaining voltage
                ; ------ Valid values 0.01 to 99
                Phase_To_Phase_RVC_Threshold_In_Percent=6
                Phase_To_Phase_RVC_Hysteresis_In_Percent=2

                ;----------------------------------------------------
                [Snapshot_Events]
                ;----------------------------------------------------
                ; ------ Valid values: OFF, 3, 6, 24
                Waveform_Snapshot_Interval_In_Hours=OFF

                ; ------ Valid values: ON, OFF
                Enable_Snapshot_Harmonics=ON
                Waveform_Snapshot_At_Startup=OFF

                ; ------ Valid values: 0-23
                Snapshot_Trigger_Hour=0

                ; ------ Valid values: ON, OFF; default = OFF
                ; ------ Enables the recording of harmonic power spectrum (CSV)
                ; -------Make sure the tag ""Recorded_Samples_Per_Cycle"" is set to 256
                Enable_Snapshot_Harmonics_Power=OFF

                ;----------------------------------------------------
                [AN1_E_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                AN1_E_Dip_Events=OFF
                AN1_E_Swell_Events=OFF
                AN1_E_Dip_Threshold_In_Volts=2.00
                AN1_E_Swell_Threshold_In_Volts=60.00
                AN1_E_Event_Hysteresis_In_Volts=0.50

                ;----------------------------------------------------
                [AN2_E_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                AN2_E_Dip_Events=OFF
                AN2_E_Swell_Events=OFF
                AN2_E_Dip_Threshold_In_Volts=10.00
                AN2_E_Swell_Threshold_In_Volts=50.00
                AN2_E_Event_Hysteresis_In_Volts=0.50

                ;----------------------------------------------------
                [AN1_AN2_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                AN1_AN2_Dip_Events=OFF
                AN1_AN2_Swell_Events=OFF
                AN1_AN2_Dip_Threshold_In_Volts=2.00
                AN1_AN2_Swell_Threshold_In_Volts=60.00
                AN1_AN2_Event_Hysteresis_In_Volts=0.50

                ;----------------------------------------------------
                [AN3_E_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                AN3_E_Dip_Events=OFF
                AN3_E_Swell_Events=OFF
                AN3_E_Dip_Threshold_In_Volts=2.00
                AN3_E_Swell_Threshold_In_Volts=60.00
                AN3_E_Event_Hysteresis_In_Volts=0.50

                ;----------------------------------------------------
                [AN4_E_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                AN4_E_Dip_Events=OFF
                AN4_E_Swell_Events=OFF
                AN4_E_Dip_Threshold_In_Volts=10.00
                AN4_E_Swell_Threshold_In_Volts=50.00
                AN4_E_Event_Hysteresis_In_Volts=0.50

                ;----------------------------------------------------
                [AN3_AN4_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                AN3_AN4_Dip_Events=OFF
                AN3_AN4_Swell_Events=OFF
                AN3_AN4_Dip_Threshold_In_Volts=2.00
                AN3_AN4_Swell_Threshold_In_Volts=60.00
                AN3_AN4_Event_Hysteresis_In_Volts=0.50

                ;----------------------------------------------------
                [Frequency_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                Frequency_Events=OFF
                Underfrequency_Threshold_In_Percent=99.50
                Overfrequency_Threshold_In_Percent=100.50
                Frequency_Event_Hysteresis_In_Percent=0.20

                ;----------------------------------------------------
                [Phase_Current_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                Phase_Current_Events=OFF
                Current_I6_Events=OFF
                Current_I7_Events=OFF
                Current_I8_Events=OFF
                Current_I9_Events=OFF
                Current_I10_Events=OFF
                Current_I11_Events=OFF
                Current_I12_Events=OFF
                Current_I13_Events=OFF
                Current_I14_Events=OFF

                ; ------ Your PQube 3 will trigger if any RMS phase current goes above the level threshold.
                Phase_Current_Level_Threshold_In_Amps=1
                Phase_Current_Level_Hysteresis_In_Amps=0.5
                Current_I6_Level_Threshold_In_Amps=1
                Current_I6_Level_Hysteresis_In_Amps=0.5
                Current_I7_Level_Threshold_In_Amps=1
                Current_I7_Level_Hysteresis_In_Amps=0.5
                Current_I8_Level_Threshold_In_Amps=1
                Current_I8_Level_Hysteresis_In_Amps=0.5
                Current_I9_Level_Threshold_In_Amps=1
                Current_I9_Level_Hysteresis_In_Amps=0.5
                Current_I10_Level_Threshold_In_Amps=1
                Current_I10_Level_Hysteresis_In_Amps=0.5
                Current_I11_Level_Threshold_In_Amps=1
                Current_I11_Level_Hysteresis_In_Amps=0.5
                Current_I12_Level_Threshold_In_Amps=1
                Current_I12_Level_Hysteresis_In_Amps=0.5
                Current_I13_Level_Threshold_In_Amps=1
                Current_I13_Level_Hysteresis_In_Amps=0.5
                Current_I14_Level_Threshold_In_Amps=1
                Current_I14_Level_Hysteresis_In_Amps=0.5

                ; ------ Your PQube 3 will trigger an inrush current event if any RMS phase current
                ; ------ increases by more than the inrush threshold, within the specified number of cycles or less.
                Phase_Current_Inrush_Threshold_In_Amps=1
                Phase_Current_Inrush_Threshold_In_Cycles=0.5
                Current_I6_Inrush_Threshold_In_Amps=1
                Current_I6_Inrush_Threshold_In_Cycles=0.5
                Current_I7_Inrush_Threshold_In_Amps=1
                Current_I7_Inrush_Threshold_In_Cycles=0.5
                Current_I8_Inrush_Threshold_In_Amps=1
                Current_I8_Inrush_Threshold_In_Cycles=0.5
                Current_I9_Inrush_Threshold_In_Amps=1
                Current_I9_Inrush_Threshold_In_Cycles=0.5
                Current_I10_Inrush_Threshold_In_Amps=1
                Current_I10_Inrush_Threshold_In_Cycles=0.5
                Current_I11_Inrush_Threshold_In_Amps=1
                Current_I11_Inrush_Threshold_In_Cycles=0.5
                Current_I12_Inrush_Threshold_In_Amps=1
                Current_I12_Inrush_Threshold_In_Cycles=0.5
                Current_I13_Inrush_Threshold_In_Amps=1
                Current_I13_Inrush_Threshold_In_Cycles=0.5
                Current_I14_Inrush_Threshold_In_Amps=1
                Current_I14_Inrush_Threshold_In_Cycles=0.5

                ;----------------------------------------------------
                [Neutral_Current_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                Neutral_Current_Events=OFF

                ; ------ Your PQube 3 will trigger if the RMS neutral current goes above the level threshold.
                Neutral_Current_Level_Threshold_In_Amps=1
                Neutral_Current_Level_Hysteresis_In_Amps=0.5

                ; ------ Your PQube 3 will trigger an inrush current event if RMS neutral current increases
                ; ------ by more than the inrush threshold, within the specified number of cycles or less.
                Neutral_Current_Inrush_Threshold_In_Amps=1
                Neutral_Current_Inrush_Threshold_In_Cycles=0.5

                ;----------------------------------------------------
                [Earth_Current_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                Earth_Current_Events=OFF

                ; ------ Your PQube 3 will trigger if the RMS earth current goes above the level threshold.
                Earth_Current_Level_Threshold_In_Amps=1
                Earth_Current_Level_Hysteresis_In_Amps=0.5

                ; ------ Your PQube 3 will trigger an inrush current event if RMS earth current increases
                ; ------ by more than the inrush threshold, within the specified number of cycles or less.
                Earth_Current_Inrush_Threshold_In_Amps=1
                Earth_Current_Inrush_Threshold_In_Cycles=0.2

                ;----------------------------------------------------
                [Major_Dip_Events]
                ;----------------------------------------------------
                ; ------ Major dips are defined by your selected depth/duration curve
                ; ------ PSL's PQ1 Power Quality Relay Emulation
                ; ------ Valid values: OFF, ITIC, CBEMA, SEMI_F47, STANDARD, SAMSUNG_POWER_VACCINE, MIL_STD_704E, MIL_STD_1399, CUSTOM
                Major_Dip_Threshold_Settings=OFF

                ; ------ Customize your Major-Dip-Ride-Through Curve by setting your own sag/dip threshold levels
                ; ------ in percent and duration. Subsequent threshold levels n percent and duration MUST be set""
                ; ------ lower in percent and shorter in duration than the previous threshold levels.""
                ; ------ Example of Valid Usage
                ; ------ Major_Dip_Threshold_Level_1_in_Percent=80
                ; ------ Major_Dip_Threshold_Level_1_Duration_in_Seconds=5
                ; ------ Major_Dip_Threshold_Level_2_in_Percent=50
                ; ------ Major_Dip_Threshold_Level_2_Duration_in_Seconds=3
                ; ------ Major_Dip_Threshold_Level_3_in_Percent=20
                ; ------ Major_Dip_Threshold_Level_3_Duration_in_Seconds=0.5
                Major_Dip_Threshold_Level_1_in_Percent=OFF
                Major_Dip_Threshold_Level_1_Duration_in_Seconds=0
                Major_Dip_Threshold_Level_2_in_Percent=OFF
                Major_Dip_Threshold_Level_2_Duration_in_Seconds=0
                Major_Dip_Threshold_Level_3_in_Percent=OFF
                Major_Dip_Threshold_Level_3_Duration_in_Seconds=0
                Major_Dip_Threshold_Level_4_in_Percent=OFF
                Major_Dip_Threshold_Level_4_Duration_in_Seconds=0

                ;----------------------------------------------------
                [Waveshape_Change_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: ON OFF
                Waveshape_Change_Events=OFF

                ; ------ These thresholds use the ""floating window"" algorithm to compare each cycle to the
                ; ------ previous cycle. It is especially useful for capturing power factor correction capacitor switching.
                Voltage_Threshold_In_Percent_Of_Nominal=20.0
                Duration_Threshold_In_Percent_Of_Cycle=10.0

                ;----------------------------------------------------
                [DIG1_Events]
                ;----------------------------------------------------
                ; ------ Valid Values: OFF/ON, default is OFF
                Enable_DIG1_Events=OFF

                ; ------ Digital input normal state (no event, value 0) can be:
                ; ------ Normally_Closed (0V) - an event is triggered, value 1, when connected relay opens)
                ; ------ Normally_Open (wetted voltage) - an event triggered, value 1 when connected relay closes)
                ; ------ Valid values: Normally_Closed, Normally_Open
                DIG1_Normal_State=Normally_Closed

                ;----------------------------------------------------
                [EnviroSensor_Probe_Events]
                ;----------------------------------------------------
                Probe_A_Overtemperature_Events=OFF
                Probe_A_Undertemperature_Events=OFF
                Probe_A_Undertemperature_Threshold_in_Deg_C=0
                Probe_A_Overtemperature_Threshold_in_Deg_C=50
                Probe_A_Temperature_Event_Hysteresis_in_Deg_C=2
                Probe_A_High_Humidity_Events=OFF
                Probe_A_Low_Humidity_Events=OFF
                Probe_A_Low_Humidity_Threshold_in_Percent_RH=5
                Probe_A_High_Humidity_Threshold_in_Percent_RH=90
                Probe_A_Humidity_Event_Hysteresis_in_Percent_RH=2
                Probe_B_Overtemperature_Events=OFF
                Probe_B_Undertemperature_Events=OFF
                Probe_B_Undertemperature_Threshold_in_Deg_C=0
                Probe_B_Overtemperature_Threshold_in_Deg_C=50
                Probe_B_Temperature_Event_Hysteresis_in_Deg_C=2
                Probe_B_High_Humidity_Events=OFF
                Probe_B_Low_Humidity_Events=OFF
                Probe_B_Low_Humidity_Threshold_in_Percent_RH=5
                Probe_B_High_Humidity_Threshold_in_Percent_RH=90
                Probe_B_Humidity_Event_Hysteresis_in_Percent_RH=2

                ;----------------------------------------------------
                [HF_Impulse_Events]
                ;----------------------------------------------------
                ; ------ High-frequency impulse detection and recording.
                ; ------ Valid values: ON, OFF, default is OFF
                HF_Impulse_Recording=OFF

                ; ------ Apply a post-event averaging DC offset removal.
                ; ------ Calibrated PQubes should not require this, but if the Impulse feature
                ; ------ develops a DC drift, this setting will remove the DC offset
                ; ------ based on post-processing.
                ; ------ Valid values: ON, OFF, default is OFF
                HF_Impulse_DC_Averaging=OFF

                ; ------ Set this tag to ON is you want to override the threshold limit to lower than 300V.
                ; ------ By default the tag is set to OFF, and the PQube3 ignores threshold limits lower than 300V
                ; ------ Valid values: ON, OFF, default is OFF
                HF_Impulse_No_Lower_Threshold_Limit=OFF

                ; ------ Recording on L1-to-Earth (4MHz) or 4 channels (L1-E, L2-E, L3-E, and N-E) (1MHz)
                ; ------ Valid Values: L1-E, L2-E, L3-E, N-E, 4-channels, default is 4-channels
                HF_Impulse_Configuration=4-channels

                ; ------ Threshold for both positive and negative transient in Volts
                ; ------ Valid values: range between 300 and 6000
                HF_Impulse_Threshold_in_Volts=2000

                ;----------------------------------------------------
                [Mains_Signaling]
                ;----------------------------------------------------
                Mains_Signaling_Events=OFF
                Mains_Signaling_Threshold_In_Volts=60
                Mains_Signaling_Harmonic_In_Hz=316.67

                ;-----------------------------------------------------
                [Relay_Configuration]
                ;-----------------------------------------------------
                Relay_2_Normal_State=Normally_Closed
                Relay_2_Enable_Latching=OFF
                Relay_3_Normal_State=Normally_Closed
                Relay_3_Enable_Latching=OFF
                Relay_4_Normal_State=Normally_Closed
                Relay_4_Enable_Latching=OFF

                ; ------ Valid values: WATTS, VOLT_AMPS, default is WATTS
                Loads_Triggering_Source=WATTS

                ; ------ Valid values: 1 - 10, default is 1
                RM8_Averaging_Period_in_Seconds=1

                RM8_P1_R01_Parameter=None
                RM8_P1_R01_Threshold=
                RM8_P1_R01_Hysteresis=
                RM8_P1_R01_Trigger_Condition=High
                RM8_P1_R01_Enable_Latching=OFF
                RM8_P1_R01_Normal_State=Normally_Closed

                RM8_P1_R02_Parameter=None
                RM8_P1_R02_Threshold=
                RM8_P1_R02_Hysteresis=
                RM8_P1_R02_Trigger_Condition=High
                RM8_P1_R02_Enable_Latching=OFF
                RM8_P1_R02_Normal_State=Normally_Closed

                RM8_P1_R03_Parameter=None
                RM8_P1_R03_Threshold=
                RM8_P1_R03_Hysteresis=
                RM8_P1_R03_Trigger_Condition=High
                RM8_P1_R03_Enable_Latching=OFF
                RM8_P1_R03_Normal_State=Normally_Closed

                RM8_P1_R04_Parameter=None
                RM8_P1_R04_Threshold=
                RM8_P1_R04_Hysteresis=
                RM8_P1_R04_Trigger_Condition=High
                RM8_P1_R04_Enable_Latching=OFF
                RM8_P1_R04_Normal_State=Normally_Closed

                RM8_P1_R05_Parameter=None
                RM8_P1_R05_Threshold=
                RM8_P1_R05_Hysteresis=
                RM8_P1_R05_Trigger_Condition=High
                RM8_P1_R05_Enable_Latching=OFF
                RM8_P1_R05_Normal_State=Normally_Closed

                RM8_P1_R06_Parameter=None
                RM8_P1_R06_Threshold=
                RM8_P1_R06_Hysteresis=
                RM8_P1_R06_Trigger_Condition=High
                RM8_P1_R06_Enable_Latching=OFF
                RM8_P1_R06_Normal_State=Normally_Closed

                RM8_P1_R07_Parameter=None
                RM8_P1_R07_Threshold=
                RM8_P1_R07_Hysteresis=
                RM8_P1_R07_Trigger_Condition=High
                RM8_P1_R07_Enable_Latching=OFF
                RM8_P1_R07_Normal_State=Normally_Closed

                RM8_P1_R08_Parameter=None
                RM8_P1_R08_Threshold=
                RM8_P1_R08_Hysteresis=
                RM8_P1_R08_Trigger_Condition=High
                RM8_P1_R08_Enable_Latching=OFF
                RM8_P1_R08_Normal_State=Normally_Closed

                ;-----------------------------------------------------
                [Event_Relay_Trigger]
                ;-----------------------------------------------------
                ; ------ Relays open for the event duration, or 3 seconds, whichever is longer.
                ; ------ You can connect any event to the relay.
                ; ------ One relay may be connected to multiple events
                ; ------ Any event can be connected to multiple relays.
                ; ------ RLY1 is standard in every PQube. RLY2, RLY3, and RLY4 are factory-installed options.
                ; ------ Examples: OFF means don't trigger a relay.
                ; ------      1 means trigger RLY1.
                ; ------      2 means trigger RLY2.
                ; ------      12 means trigger both RLY1 and RLY2.
                Trigger_Relay_On_Dip=OFF
                Trigger_Relay_On_Major_Dip=OFF
                Trigger_Relay_On_Swell=OFF
                Trigger_Relay_On_Interruption=OFF
                Trigger_Relay_On_AN1_Dip=OFF
                Trigger_Relay_On_AN1_Swell=OFF
                Trigger_Relay_On_AN2_Dip=OFF
                Trigger_Relay_On_AN2_Swell=OFF
                Trigger_Relay_On_AN1_AN2_Dip=OFF
                Trigger_Relay_On_AN1_AN2_Swell=OFF
                Trigger_Relay_On_AN3_Dip=OFF
                Trigger_Relay_On_AN3_Swell=OFF
                Trigger_Relay_On_AN4_Dip=OFF
                Trigger_Relay_On_AN4_Swell=OFF
                Trigger_Relay_On_AN3_AN4_Dip=OFF
                Trigger_Relay_On_AN3_AN4_Swell=OFF
                Trigger_Relay_On_DIG1_Dip=OFF
                Trigger_Relay_On_DIG1_Swell=OFF
                Trigger_Relay_On_Underfrequency=OFF
                Trigger_Relay_On_Overfrequency=OFF
                Trigger_Relay_On_Impulse=OFF
                Trigger_Relay_On_Snapshot=OFF
                Trigger_Relay_On_Waveshape_Change=OFF
                Trigger_Relay_On_Phase_Current=OFF
                Trigger_Relay_On_Neutral_Current=OFF
                Trigger_Relay_On_Earth_Current=OFF
                Trigger_Relay_On_I6_Current=OFF
                Trigger_Relay_On_I7_Current=OFF
                Trigger_Relay_On_I8_Current=OFF
                Trigger_Relay_On_I9_Current=OFF
                Trigger_Relay_On_I10_Current=OFF
                Trigger_Relay_On_I11_Current=OFF
                Trigger_Relay_On_I12_Current=OFF
                Trigger_Relay_On_I13_Current=OFF
                Trigger_Relay_On_I14_Current=OFF
                Trigger_Relay_On_Voltage_2_L1_Dip=OFF
                Trigger_Relay_On_Voltage_2_L1_Swell=OFF
                Trigger_Relay_On_Voltage_2_L2_Dip=OFF
                Trigger_Relay_On_Voltage_2_L2_Swell=OFF
                Trigger_Relay_On_Voltage_2_L3_Dip=OFF
                Trigger_Relay_On_Voltage_2_L3_Swell=OFF

                ;----------------------------------------------------
                [Network_Setup]
                ;----------------------------------------------------
                ; ------ Valid Values: Use_DHCP, Use_Fixed_IP
                IP_Address_Method=Use_DHCP
                Publish_IP_Address=ON

                ;----------------------------------------------------
                [Fixed_IP]
                ;----------------------------------------------------
                ; ------ This section is ignored if the IP_Address_Method is set to Use_DHCP
                IP_Address=172.17.69.20
                IP_Gateway=172.17.1.1
                IP_Mask=255.255.0.0
                IP_DNS1=8.8.8.8
                IP_DNS2=8.8.4.4

                ;----------------------------------------------------
                [Email_Server_Settings]
                ;----------------------------------------------------
                ; ------ Incoming Email Server Settings
                POP_Email_Server_Address=mail.pqube.com
                POP_Email_Server_Port=110
                Incoming_Email_Protocol=POP3

                ; ------ Connection security SSL/TLS
                ; ------ Valid values: ON, OFF
                Incoming_Email_SSL_Encryption=ON

                ; ------ Outgoing Email Server Settings
                SMTP_Server_Address=[mail.pqube.com]
                SMTP_Server_Port=587

                ; ------ Connection security SSL/TLS
                ; ------ Valid values: ON, OFF
                Outgoing_Email_SSL_Encryption=ON

                ; ------ IMPORTANT Your PQube needs its own e-mail account.
                ; ------ Do not assign your personal e-mail account to your PQube.
                ; ------ Your PQube automatically clears out the inbox after processing email commands.
                ; ------ User account and password for the server to fetch emails from
                ; ------ Example Account is p300xxxx@pqube.com, password is p300xxxx
                PQube_Email_Account=""""
                PQube_Email_Password=""""

                ; ------ Use pre-programmed email server settings. Valid values: PSL, GMAIL, OTHER
                Email_Address_Provider=PSL

                ;----------------------------------------------------
                [Email_Commands_To_PQube3]
                ;----------------------------------------------------
                ; ------ Enable email commands (incoming emails to your PQube 3)
                ; ------ Valid values: ON, OFF
                Email_Commands=OFF

                ; ------ How often your PQube 3 checks the inbox for new email commands
                Check_Every_N_Seconds=300

                ; ------ Make sure this is the first word in your email subject, or your email command will be rejected.
                ; ------ Valid values: Words up to x characters, no spaces.
                Subject_Must_Begin_With=""PQube3""

                ; ------ Your PQube 3 only accepts email commands from the email addresses in this list.
                Email_Must_Be_From_1=""""
                Email_Must_Be_From_2=""""
                Email_Must_Be_From_3=""""
                Email_Must_Be_From_4=""""
                Email_Must_Be_From_5=""""

                ; ------ Your PQube 3 ignores emails from email addresses that contain the following keywords, and are not on the Email_Must_Be_From list.
                ; ------ Ignored emails do not trigger notifications to be sent to the email postmaster.
                ; ------ Valid keywords are 2 to x characters.
                Ignore_Sender_Containing_1=""""
                Ignore_Sender_Containing_2=""""
                Ignore_Sender_Containing_3=""""
                Ignore_Sender_Containing_4=""""
                Ignore_Sender_Containing_5=""""

                ;----------------------------------------------------
                [Email_Notifications_From_PQube3]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                ; ------ Summary emails are short, plain-text event notifications.
                Enable_Event_Summary_Email=OFF
                Send_Reset_Emails=OFF
                Send_Events_Emails=OFF
                Send_Trends_Emails=OFF
                Send_Snapshot_Emails=OFF

                ; ------ Valid Values: ON, OFF, default is ON
                Send_CSV_in_Emails=ON

                ; ------ Valid Values: Human_Readable_HTML,  Human_Readable_Text,  Machine_Readable_XML
                Email_Body_Type=Human_Readable_HTML

                ; ------ Distribution list of summary emails
                Event_Summary_Email_To_1=""""
                Event_Summary_Email_To_2=""""
                Event_Summary_Email_To_3=""""

                ; ------ CC distribution list of summary emails
                Event_Summary_Email_CC_1=""""
                Event_Summary_Email_CC_2=""""
                Event_Summary_Email_CC_3=""""

                ; ------ BCC distribution list of summary emails
                Event_Summary_Email_BCC_1=""""
                Event_Summary_Email_BCC_2=""""
                Event_Summary_Email_BCC_3=""""

                ; ------ Distribution list of emails
                Email_To_1=""""
                Email_To_2=""""
                Email_To_3=""""
                Email_To_4=""""
                Email_To_5=""""

                ; ------ CC Distribution list of emails
                Email_CC_1=""""
                Email_CC_2=""""
                Email_CC_3=""""
                Email_CC_4=""""
                Email_CC_5=""""

                ; ------ BCC Distribution list of emails
                Email_BCC_1=""""
                Email_BCC_2=""""
                Email_BCC_3=""""
                Email_BCC_4=""""
                Email_BCC_5=""""

                ; ------ Send email errors to the following email address
                Email_Errors_To=

                ; ------ Valid values ON, OFF
                Flush_Emails_on_Restart=OFF

                ;----------------------------------------------------
                [Modbus]
                ;----------------------------------------------------
                ; ------ Whether to turn on the modbus server at startup
                ; ------ Valid values ON, OFF
                Enable_Modbus=ON
                Modbus_Slave_Device_Address=1
                Modbus_TCP_Port=502
                Modbus_Register_Start_Address=7000
                Modbus_Byte_Order=BIG_ENDIAN

                ;----------------------------------------------------
                [Output_Formatting]
                ;----------------------------------------------------
                Decimal_Separator="".""
                Date_Separator=""/""
                Time_Separator="":""
                CSV_Separator="",""

                ;----------------------------------------------------
                [SNMP_Settings]
                ;----------------------------------------------------
                ; ------ Enable SNMP Polling. Valid Values: ON, OFF
                SNMPD_Polling=OFF

                ; ------ Enable SNMP Traps. Valid Values: ON, OFF
                SNMPD_Traps=OFF

                ; ------ SNMP Trap Server IP address
                ; ------ Valid values: IP address
                SNMP_Trap_Server=
                SNMP_Port=161

                ; ------ Valid Values: v2c, v3
                SNMP_Trap_Version=v3

                ; ------ Parameters specific to TRAP v2
                SNMP_V1_V2_Community_Name=""public""

                ; ------ This is the node's fully-qualified domain name (OID: 1.3.6.1.2.1.1.5)
                ; ------ If the tag is empty it will show PQube's serial number.
                SNMP_SysName=localhost.localdomain
                ; ------ PQube's physical location (OID: 1.3.6.1.2.1.1.6)
                SNMP_SysLocation=""pqube-1""

                ; ------ Parameters specific to TRAP v3
                ; ------ Valid Values: noAuthNoPriv, authNoPriv, authPriv
                SNMP_V3_Security_Level=noAuthNoPriv
                SNMP_V3_User_Name=""MD5DESUser""
                SNMP_V3_Auth_Protocol=MD5
                SNMP_V3_Auth_Password=""MD5UserPassword""
                SNMP_V3_Priv_Protocol=DES
                SNMP_V3_Priv_Password=""DESUserPassword""

                ;----------------------------------------------------
                [DNP3]
                ;----------------------------------------------------
                ; ------ Whether to turn on the DNP3 service at startup
                ; ------ Valid values ON, OFF, default OFF
                Enable_DNP3=OFF

                ; ------ Configure the DNP3 outstation listening port.  Default 20000.
                DNP3_Port=20000

                ; ------ Enable the DNP3 outstation source ID.  Default 4.
                DNP3_Source_Id=4

                ; ------ Enable the DNP3 slave outstation client SA v2.  Default OFF.
                ; ------ 1 = AES128 Symmetric Key
                ; ------ 2 = AES256 Symmetric Key
                DNP3_Enable_Secure_Authentication=OFF

                ;----------------------------------------------------
                [BACnet_Settings]
                ;----------------------------------------------------
                ; ------ Valid values ON, OFF, default OFF
                Enable_BACnet=OFF
                ; ------ Valid values [1-65535], default is 47808
                BACnet_Port=47808

                ;----------------------------------------------------
                [SNTP_Settings]
                ;----------------------------------------------------
                Enable_SNTP=ON
                SNTP_Server=2.pool.ntp.org
                ; ------ Valid Values: 1 to 168 (1 week)
                SNTP_Update_Interval_In_Hours=1

                ;----------------------------------------------------
                [NTP_Settings]
                ;----------------------------------------------------
                Enable_NTP=OFF
                NTP_Server=north-america.pool.ntp.org
                ; ------ Valid Values: 1 to 168 (1 week)
                NTP_Update_Interval_In_Hours=1

                ;----------------------------------------------------
                [HTTP_Web_Server_Settings]
                ;----------------------------------------------------
                ; ------ Makes the Command page visible
                ; ------ Valid values ON, OFF
                HTTP_Web_Server_Commands_Page=ON

                ; ------ HTTP port used to access the PQube3 web server pages
                ; ------ Valid values [1-65535], default is 80
                ; ------ make sure that the port chosen does not conflict with
                ; ------ reserved ports and other PQube3 services ports
                ; ------ e.g. FTP, SNMP, SSH, Web Command page...
                HTTP_Web_Server_Port=80

                ; ------ HTTP port used to access the PQube3 web server Command page
                ; ------ Valid values [1-65535], default is 8888
                ; ------ make sure that the port chosen does not conflict with
                ; ------ reserved ports and other PQube3 services ports
                ; ------ e.g. FTP, SNMP, SSH, Web server, HTTP ...
                ; ------ the following tag is deprecated with firmware 3.7.9 and above
                HTTP_Web_Server_Command_Port=8888

                ; ------ Require password to access the PQube3 web server pages (except command page)
                Require_HTTP_Authorization=OFF
                HTTP_User_Name=""""
                HTTP_Password=""""

                ; ------ Require password to access the PQube3 web server command page
                Require_HTTP_Admin_Authorization=OFF
                HTTP_Admin_User_Name=""""
                HTTP_Admin_Password=""""

                ; ------ X-Frame-Options is an HTTP security header that provides
                ; ------ clickjacking protection by not allowing rendering of a page in a frame.
                ; ------ Valid values OFF, DENY, SAMEORIGIN
                ; ------ OFF: Disables security header
                ; ------ DENY: Disables the loading of the page in a frame. Default option.
                ; ------ SAMEORIGIN: Allows the page to be loaded in a frame on the same origin as the page itself.
                HTTP_X_Frame_Options=DENY

                ;----------------------------------------------------
                [FTP_Settings]
                ;----------------------------------------------------
                ; ------ FTP control and Data ports, the default control port is 21, the default data port is 20
                ; ------ Valid values  [1-65535]
                ; ------ make sure that the port chosen does not conflict with
                ; ------ reserved ports and other PQube3 services ports
                ; ------ e.g. SNMP, SSH, Web server, HTTP ...
                FTP_Control_Port=21
                FTP_Data_Port=20

                ; ------ There are 3 user accounts for downloading data : ""ftp_user_1"" , ""ftp_user_2"" and ""ftp_user_3""
                ; ------ Account ""ftp_config"" - for reading and sending the PQube3 setup.ini files
                ; ------ Account ""ftp_updater"" - for sending firmware updates
                ; ------ If no password is assigned for a profile, the user does not have access
                ; ------ Password must be at least 8 characters
                ; ------ Password for profile ""ftp_user_1""
                FTP_Password_1=""""

                ; ------ Password for profile ""ftp_user_2""
                FTP_Password_2=""""

                ; ------ Password for profile ""ftp_user_3""
                FTP_Password_3=""""

                ; ------ Password for profile ""ftp_config""
                FTP_Password_4=""""

                ; ------ Password for profile ""ftp_updater""
                FTP_Password_5=""""

                ; ------ Upon event trigger, PQube 3 generates a compressed (.tar.gz) file from event files
                ; ------ and pushes it to the distant server via FTP.
                Enable_FTP_Push=OFF
                FTP_Push_Server=
                FTP_Push_User_Name=""""
                FTP_Push_Password=""""

                ;----------------------------------------------------
                [Security_Settings]
                ;----------------------------------------------------
                ; ------ This enables or disables the embedded PQube3 firewall.
                ; ------ The firewall will close all ports except the ports being used by the PQube3.
                ; ------ The firewall will limit the number of external attempts to open ports for a given period of time,
                ; ------ therefore protecting against attacks from the Internet.
                ; ------ Valid values: ON, OFF, default is ON
                Enable_Firewall=ON

                ; ------ Turning this Security tag ON makes the PQube3 web pages accessible only via secure HTTP (HTTPS)
                ; ------ this applies to all pages, including the command page
                ; ------ Turning this Security tag ON changes the HTTPS port to 443
                ; ------ If the tag is turned OFF, then the Web pages are accessible via non secure (HTTP)
                ; ------ Valid values: ON, OFF, default is OFF
                Require_WebServer_Security=OFF

                ; ------ Turning this tag IMPLICIT or EXPLICIT makes the PQube3 FTP server accessible only via secure FTP (FTPS)
                ; ------ this applies to all FTP profiles (user, config, and update profiles)
                ; ------ the FTP ports specified in the FTP section are used
                ; ------ See the FTP_Settings section for more information about profiles
                ; ------ If the tag is turned OFF, then the Web pages are accessible via non secure (FTP) or secure (FTPS)
                ; ------ Valid values: OFF, IMPLICIT, EXPLICIT, default is OFF
                Require_FTP_Security=OFF

                ; ------ Valid values: ON, OFF, default is OFF
                ; ------ Enables access control to the PQube 3 display screen
                Enable_Display_Passcode=OFF

                ; ------ Passcode 4 digits (if function enabled)
                ; ------ Enter a sequence of 4 digits. Valid digits: 0 – 9, default ""0000""
                Display_Passcode=0000

                ;----------------------------------------------------
                [Trend_Settings]
                ;----------------------------------------------------
                ; ------ Valid Values: ON, OFF
                Enable_Daily_Trends=ON
                Enable_Weekly_Trends=ON
                Enable_Monthly_Trends=ON

                ; ------ Individual phase recordings for voltage, current, power.
                ; ------ Valid Values: AUTO, ON, OFF
                Trend_Individual_Phases=AUTO

                ; ------ Valid Values: ON, OFF
                Omit_IEC_Flagged_Data_From_Stats=ON

                ; ------ AUTO uses voltage dip/swell thresholds
                Min_Volts_of_Interest_in_Percent_of_Nominal=95
                Max_Volts_of_Interest_in_Percent_of_Nominal=105

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_Current_of_Interest_in_Amps=0
                Max_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_Neutral_Current_of_Interest_in_Amps=0
                Max_Neutral_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_Earth_Current_of_Interest_in_Amps=0
                Max_Earth_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I6_Current_of_Interest_in_Amps=0
                Max_I6_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I7_Current_of_Interest_in_Amps=0
                Max_I7_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I8_Current_of_Interest_in_Amps=0
                Max_I8_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I9_Current_of_Interest_in_Amps=0
                Max_I9_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I10_Current_of_Interest_in_Amps=0
                Max_I10_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I11_Current_of_Interest_in_Amps=0
                Max_I11_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I12_Current_of_Interest_in_Amps=0
                Max_I12_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I13_Current_of_Interest_in_Amps=0
                Max_I13_Current_of_Interest_in_Amps=AUTO

                ; ------ AUTO uses 0 for Min and Full-Scale current for Max (typically the primary rating of your CT)
                Min_I14_Current_of_Interest_in_Amps=0
                Max_I14_Current_of_Interest_in_Amps=AUTO


                ; ------ AUTO uses under/over frequency event thresholds
                Min_Frequency_of_Interest_in_Percent_of_Nominal=99
                Max_Frequency_of_Interest_in_Percent_of_Nominal=101

                ; ------ AUTO uses 0 for Min and 10 times Analog Ratio for Max
                Min_AN1_E_of_Interest_in_RMS_volts=0
                Max_AN1_E_of_Interest_in_RMS_volts=10
                Min_AN2_E_of_Interest_in_RMS_volts=0
                Max_AN2_E_of_Interest_in_RMS_volts=10
                Min_AN1_AN2_of_Interest_in_RMS_volts=0
                Max_AN1_AN2_of_Interest_in_RMS_volts=10
                Min_AN3_E_of_Interest_in_RMS_volts=0
                Max_AN3_E_of_Interest_in_RMS_volts=10
                Min_AN4_E_of_Interest_in_RMS_volts=0
                Max_AN4_E_of_Interest_in_RMS_volts=10
                Min_AN3_AN4_of_Interest_in_RMS_volts=0
                Max_AN3_AN4_of_Interest_in_RMS_volts=10

                ; ------ Focus the region of interest for powers  to the positive, negative scale,
                ; ------ or covers both positive and negative
                ; ------ This also sets your min/max Power Factor of interest
                ; ------ Valid Values: Positive, Negative, BOTH
                Power_Polarity_of_Interest=BOTH

                ; ------ Focus the region of interest for analog power (AN1XAN2 and AN3XAN4) to the positive scale, negative scale,
                ; ------ or covers both positive and negative
                ; ------ Valid Values: Positive, Negative, BOTH
                AN_Power_Polarity_of_Interest=BOTH

                ; ------ Valid Values: from -20 up to 80
                Min_Temperature_of_Interest_in_DegC=0
                Max_Temperature_of_Interest_in_DegC=50
                Min_Humidity_of_Interest_in_%_RH=0
                Max_Humidity_of_Interest_in_%_RH=100
                Min_Baro_Pressure_of_Interest_in_hPa=950
                Max_Baro_Pressure_of_Interest_in_hPa=1050

                ; ------ Focus the region of interest for the trends of vector acceleration
                ; ------ Valid values: 0m/s2 - 50m/s2 - values must positive values
                ; ------ when the option remove gravity is ON, typical values can be [0m/s2, 0.5m/s2]
                ; ------ when the option remove gravity is OFF, typical values can be [9m/s2, 11m/s2]
                Min_Vector_Acceleration_of_Interest_in_m_per_second2=0.0
                Max_Vector_Acceleration_of_Interest_in_m_per_second2=0.5
                Max_Voltage_Unbalance_of_Interest_in_Percent=10
                Max_Current_Unbalance_of_Interest_in_Percent=99
                Max_Voltage_THD_of_Interest_in_Percent=10
                Max_Current_TDD_of_Interest_in_Percent=25
                Max_Flicker_of_Interest=4

                Max_2-150kHz_Conducted_Emissions_of_Interest_in_Volt=1.00
                Max_2-9kHz_Conducted_Emissions_of_Interest_in_Volt=1.00
                Phase_Aggregation_2-150kHz_GIF=OFF

                ;----------------------------------------------------
                [Report_Generation]
                ;----------------------------------------------------
                Enable_Report_Generator=ON
                Report_Coverage_Percentage_Requirement=80

                ; ------ Refer to EN50160 for description of the different options
                ; ------ Valid values: ""EN50160_Low_Voltage_Sync"", ""EN50160_Low_Voltage_NoSync"",
                ; ------ ""EN50160_Med_Voltage_Sync"", ""EN50160_Med_Voltage_NoSync""
                Report_Template=""EN50160_Low_Voltage_Sync""

                ; ------ This tag will allow the user to customize the embedded report generated by the PQube3.
                ; ------ Valid values: OFF, ITIC, CBEMA, SEMI_F47, STANDARD, SAMSUNG_POWER_VACCINE, MIL_STD_704E, MIL_STD_1399
                ; ------ Default is ITIC
                Embedded_Major_Sag_Report_Setting=ITIC

                [Demo_Tags]
                Display_Web_page=OFF

                [Demo_PQube_Features]
                Enable_IEC_61000-4-30_PQDIF_Generation=OFF

                ;----------------------------------------------------
                [IOT]
                ;----------------------------------------------------
                IoT_Method=""MQTT""
                IoT_Port=""8883""
                IoT_Server=""a16t5zj80xgmul-ats.iot.us-east-1.amazonaws.com""
                IOT_BLE_Addr=""980 Atlantic Avenue, Alameda, CA""
                IOT_BLE_Email_Inst=""support@powerstandards.com""
                IOT_BLE_Name=""Engineering""
                IOT_BLE_Company=""Powerside""
                IOT_BLE_Gen=""Not yet initialized through commissioning""
                IOT_BLE_Phone=""510-522-4400""
                IOT_BLE_Cord=""0.0000,0.0000""
                IOT_BLE_Meas_Point=""Uninitialized""
                ";
        }
    }
}