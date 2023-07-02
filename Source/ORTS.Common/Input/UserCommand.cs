﻿namespace ORTS.Common.Input
{
    /// <summary>
    /// Specifies game commands.
    /// </summary>
    /// <remarks>
    /// <para>The ordering and naming of these commands is important. They are listed in the UI in the order they are defined in the code, and the first word of each command is the "group" to which it belongs.</para>
    /// </remarks>
    public enum UserCommand
    {
        [GetString("Game Pause Menu")] GamePauseMenu,
        [GetString("Game Save")] GameSave,
        [GetString("Game Quit")] GameQuit,
        [GetString("Game Pause")] GamePause,
        [GetString("Game Screenshot")] GameScreenshot,
        [GetString("Game Fullscreen")] GameFullscreen,
        [GetString("Game External Controller (RailDriver)")] GameExternalCabController,
        [GetString("Game Switch Ahead")] GameSwitchAhead,
        [GetString("Game Switch Behind")] [GetParticularString("SwitchPanel", "Switch Behind")] GameSwitchBehind,
        [GetString("Game Switch Picked")] [GetParticularString("SwitchPanel", "")] GameSwitchPicked,
        [GetString("Game Signal Picked")] [GetParticularString("SwitchPanel", "")] GameSignalPicked,
        [GetString("Game Switch With Mouse")] [GetParticularString("SwitchPanel", "")] GameSwitchWithMouse,
        [GetString("Game Uncouple With Mouse")] [GetParticularString("SwitchPanel", "")] GameUncoupleWithMouse,
        [GetString("Game Change Cab")] [GetParticularString("SwitchPanel", "Change Cab")] GameChangeCab,
        [GetString("Game Request Control")] [GetParticularString("SwitchPanel", "")] GameRequestControl,
        [GetString("Game Multi Player Dispatcher")] [GetParticularString("SwitchPanel", "Map")] GameMultiPlayerDispatcher,
        [GetString("Game Multi Player Texting")] [GetParticularString("SwitchPanel", "")] GameMultiPlayerTexting,
        [GetString("Game Switch Manual Mode")] [GetParticularString("SwitchPanel", "Switch Manual")] GameSwitchManualMode,
        [GetString("Game Reset Out Of Control Mode")] [GetParticularString("SwitchPanel", "")] GameResetOutOfControlMode,
        [GetString("Game Clear Signal Forward")] [GetParticularString("SwitchPanel", "Clear Signal Forward")] GameClearSignalForward,
        [GetString("Game Clear Signal Backward")] [GetParticularString("SwitchPanel", "")] GameClearSignalBackward,
        [GetString("Game Reset Signal Forward")] [GetParticularString("SwitchPanel", "")] GameResetSignalForward,
        [GetString("Game Reset Signal Backward")] [GetParticularString("SwitchPanel", "")] GameResetSignalBackward,
        [GetString("Game Autopilot Mode")] [GetParticularString("SwitchPanel", "Autopilot")] GameAutopilotMode,
        [GetString("Game Suspend Old Player")] [GetParticularString("SwitchPanel", "")] GameSuspendOldPlayer,

        [GetString("Display Next Window Tab")] [GetParticularString("SwitchPanel", "")] DisplayNextWindowTab,
        [GetString("Display Help Window")] [GetParticularString("SwitchPanel", "")] DisplayHelpWindow,
        [GetString("Display Track Monitor Window")] [GetParticularString("SwitchPanel", "Track Monitor")] DisplayTrackMonitorWindow,
        [GetString("Display HUD")] [GetParticularString("SwitchPanel", "HUD")] DisplayHUD,
        [GetString("Display Train Driving Window")] [GetParticularString("SwitchPanel", "Train Driving")] DisplayTrainDrivingWindow,
        [GetString("Display Multi Player Window")] [GetParticularString("SwitchPanel", "")] DisplayMultiPlayerWindow,
        [GetString("Display Car Labels")] [GetParticularString("SwitchPanel", "")] DisplayCarLabels,
        [GetString("Display Station Labels")] [GetParticularString("SwitchPanel", "")] DisplayStationLabels,
        [GetString("Display Switch Window")] [GetParticularString("SwitchPanel", "Switch")] DisplaySwitchWindow,
        [GetString("Display Train Operations Window")] [GetParticularString("SwitchPanel", "Train Operations")] DisplayTrainOperationsWindow,
        [GetString("Display Train Dpu Window")] [GetParticularString("SwitchPanel", "Train Dpu")] DisplayTrainDpuWindow,
        [GetString("Display Next Station Window")] [GetParticularString("SwitchPanel", "Next Station")] DisplayNextStationWindow,
        [GetString("Display Compass Window")] [GetParticularString("SwitchPanel", "")] DisplayCompassWindow,
        [GetString("Display Train List Window")] [GetParticularString("SwitchPanel", "Train List")] DisplayTrainListWindow,
        [GetString("Display EOT List Window")] [GetParticularString("SwitchPanel", "EOT List")] DisplayEOTListWindow,

        [GetString("Debug Speed Up")] [GetParticularString("SwitchPanel", "")] DebugSpeedUp,
        [GetString("Debug Speed Down")] [GetParticularString("SwitchPanel", "")] DebugSpeedDown,
        [GetString("Debug Speed Reset")] [GetParticularString("SwitchPanel", "")] DebugSpeedReset,
        [GetString("Debug Overcast Increase")] [GetParticularString("SwitchPanel", "")] DebugOvercastIncrease,
        [GetString("Debug Overcast Decrease")] [GetParticularString("SwitchPanel", "")] DebugOvercastDecrease,
        [GetString("Debug Fog Increase")] [GetParticularString("SwitchPanel", "")] DebugFogIncrease,
        [GetString("Debug Fog Decrease")] [GetParticularString("SwitchPanel", "")] DebugFogDecrease,
        [GetString("Debug Precipitation Increase")] [GetParticularString("SwitchPanel", "")] DebugPrecipitationIncrease,
        [GetString("Debug Precipitation Decrease")] [GetParticularString("SwitchPanel", "")] DebugPrecipitationDecrease,
        [GetString("Debug Precipitation Liquidity Increase")] [GetParticularString("SwitchPanel", "")] DebugPrecipitationLiquidityIncrease,
        [GetString("Debug Precipitation Liquidity Decrease")] [GetParticularString("SwitchPanel", "")] DebugPrecipitationLiquidityDecrease,
        [GetString("Debug Weather Change")] [GetParticularString("SwitchPanel", "")] DebugWeatherChange,
        [GetString("Debug Clock Forwards")] [GetParticularString("SwitchPanel", "")] DebugClockForwards,
        [GetString("Debug Clock Backwards")] [GetParticularString("SwitchPanel", "")] DebugClockBackwards,
        [GetString("Debug Logger")] [GetParticularString("SwitchPanel", "")] DebugLogger,
        [GetString("Debug Lock Shadows")] [GetParticularString("SwitchPanel", "")] DebugLockShadows,
        [GetString("Debug Dump Keyboard Map")] [GetParticularString("SwitchPanel", "")] DebugDumpKeymap,
        [GetString("Debug Log Render Frame")] [GetParticularString("SwitchPanel", "")] DebugLogRenderFrame,
        [GetString("Debug Tracks")] [GetParticularString("SwitchPanel", "")] DebugTracks,
        [GetString("Debug Signalling")] [GetParticularString("SwitchPanel", "")] DebugSignalling,
        [GetString("Debug Reset Wheel Slip")] [GetParticularString("SwitchPanel", "")] DebugResetWheelSlip,
        [GetString("Debug Toggle Advanced Adhesion")] [GetParticularString("SwitchPanel", "")] DebugToggleAdvancedAdhesion,
        [GetString("Debug Sound Form")] [GetParticularString("SwitchPanel", "")] DebugSoundForm,
        [GetString("Debug Physics Form")] [GetParticularString("SwitchPanel", "")] DebugPhysicsForm,
        [GetString("Debug Toggle Confirmations")] [GetParticularString("SwitchPanel", "")] DebugToggleConfirmations,

        [GetString("Camera Cab")] [GetParticularString("SwitchPanel", "")] CameraCab,
        [GetString("Camera Change Passenger Viewpoint")] [GetParticularString("SwitchPanel", "")] CameraChangePassengerViewPoint,
        [GetString("Camera Toggle 3D Cab")] [GetParticularString("SwitchPanel", "")] CameraToggleThreeDimensionalCab,
        [GetString("Camera Toggle Show Cab")] [GetParticularString("SwitchPanel", "")] CameraToggleShowCab,
        [GetString("Camera Toggle Letterbox Cab")] [GetParticularString("SwitchPanel", "")] CameraToggleLetterboxCab,
        [GetString("Camera Head Out Forward")] [GetParticularString("SwitchPanel", "")] CameraHeadOutForward,
        [GetString("Camera Head Out Backward")] [GetParticularString("SwitchPanel", "")] CameraHeadOutBackward,
        [GetString("Camera Outside Front")] [GetParticularString("SwitchPanel", "")] CameraOutsideFront,
        [GetString("Camera Outside Rear")] [GetParticularString("SwitchPanel", "")] CameraOutsideRear,
        [GetString("Camera Trackside")] [GetParticularString("SwitchPanel", "")] CameraTrackside,
        [GetString("Camera SpecialTracksidePoint")] [GetParticularString("SwitchPanel", "")] CameraSpecialTracksidePoint,
        [GetString("Camera Passenger")] [GetParticularString("SwitchPanel", "")] CameraPassenger,
        [GetString("Camera Brakeman")] [GetParticularString("SwitchPanel", "")] CameraBrakeman,
        [GetString("Camera Free")] [GetParticularString("SwitchPanel", "")] CameraFree,
        [GetString("Camera Previous Free")] [GetParticularString("SwitchPanel", "")] CameraPreviousFree,
        [GetString("Camera Reset")] [GetParticularString("SwitchPanel", "")] CameraReset,
        [GetString("Camera Move Fast")] [GetParticularString("SwitchPanel", "")] CameraMoveFast,
        [GetString("Camera Move Slow")] [GetParticularString("SwitchPanel", "")] CameraMoveSlow,
        [GetString("Camera Pan (Rotate) Left")] [GetParticularString("SwitchPanel", "")] CameraPanLeft,
        [GetString("Camera Pan (Rotate) Right")] [GetParticularString("SwitchPanel", "")] CameraPanRight,
        [GetString("Camera Pan (Rotate) Up")] [GetParticularString("SwitchPanel", "")] CameraPanUp,
        [GetString("Camera Pan (Rotate) Down")] [GetParticularString("SwitchPanel", "")] CameraPanDown,
        [GetString("Camera Zoom In (Move Z)")] [GetParticularString("SwitchPanel", "")] CameraZoomIn,
        [GetString("Camera Zoom Out (Move Z)")] [GetParticularString("SwitchPanel", "")] CameraZoomOut,
        [GetString("Camera Rotate (Pan) Left")] [GetParticularString("SwitchPanel", "")] CameraRotateLeft,
        [GetString("Camera Rotate (Pan) Right")] [GetParticularString("SwitchPanel", "")] CameraRotateRight,
        [GetString("Camera Rotate (Pan) Up")] [GetParticularString("SwitchPanel", "")] CameraRotateUp,
        [GetString("Camera Rotate (Pan) Down")] [GetParticularString("SwitchPanel", "")] CameraRotateDown,
        [GetString("Camera Car Next")] [GetParticularString("SwitchPanel", "")] CameraCarNext,
        [GetString("Camera Car Previous")] [GetParticularString("SwitchPanel", "")] CameraCarPrevious,
        [GetString("Camera Car First")] [GetParticularString("SwitchPanel", "")] CameraCarFirst,
        [GetString("Camera Car Last")] [GetParticularString("SwitchPanel", "")] CameraCarLast,
        [GetString("Camera Jumping Trains")] [GetParticularString("SwitchPanel", "")] CameraJumpingTrains,
        [GetString("Camera Jump Back Player")] [GetParticularString("SwitchPanel", "")] CameraJumpBackPlayer,
        [GetString("Camera Jump See Switch")] [GetParticularString("SwitchPanel", "")] CameraJumpSeeSwitch,
        [GetString("Camera Vibrate")] [GetParticularString("SwitchPanel", "")] CameraVibrate,
        [GetString("Camera Scroll Right")] [GetParticularString("SwitchPanel", "")] CameraScrollRight,
        [GetString("Camera Scroll Left")] [GetParticularString("SwitchPanel", "")] CameraScrollLeft,
        [GetString("Camera Browse Backwards")] [GetParticularString("SwitchPanel", "")] CameraBrowseBackwards,
        [GetString("Camera Browse Forwards")] [GetParticularString("SwitchPanel", "")] CameraBrowseForwards,

        [GetString("Control Forwards")] [GetParticularString("SwitchPanel", "Direction")] ControlForwards,
        [GetString("Control Backwards")] [GetParticularString("SwitchPanel", "")] ControlBackwards,
        [GetString("Control Throttle Increase")] [GetParticularString("SwitchPanel", "")] ControlThrottleIncrease,
        [GetString("Control Throttle Decrease")] [GetParticularString("SwitchPanel", "")] ControlThrottleDecrease,
        [GetString("Control Throttle Zero")] [GetParticularString("SwitchPanel", "")] ControlThrottleZero,
        [GetString("Control Gear Up")] [GetParticularString("SwitchPanel", "Gear")] ControlGearUp,
        [GetString("Control Gear Down")] [GetParticularString("SwitchPanel", "")] ControlGearDown,
        [GetString("Control Train Brake Increase")] [GetParticularString("SwitchPanel", "")] ControlTrainBrakeIncrease,
        [GetString("Control Train Brake Decrease")] [GetParticularString("SwitchPanel", "")] ControlTrainBrakeDecrease,
        [GetString("Control Train Brake Zero")] [GetParticularString("SwitchPanel", "")] ControlTrainBrakeZero,
        [GetString("Control Engine Brake Increase")] [GetParticularString("SwitchPanel", "")] ControlEngineBrakeIncrease,
        [GetString("Control Engine Brake Decrease")] [GetParticularString("SwitchPanel", "")] ControlEngineBrakeDecrease,
        [GetString("Control Brakeman Brake Increase")] [GetParticularString("SwitchPanel", "")] ControlBrakemanBrakeIncrease,
        [GetString("Control Brakeman Brake Decrease")] [GetParticularString("SwitchPanel", "")] ControlBrakemanBrakeDecrease,
        [GetString("Control Dynamic Brake Increase")] [GetParticularString("SwitchPanel", "")] ControlDynamicBrakeIncrease,
        [GetString("Control Dynamic Brake Decrease")] [GetParticularString("SwitchPanel", "")] ControlDynamicBrakeDecrease,
        [GetString("Control Bail Off")] [GetParticularString("SwitchPanel", "")] ControlBailOff,
        [GetString("Control Brake Quick Release")] [GetParticularString("SwitchPanel", "")] ControlBrakeQuickRelease,
        [GetString("Control Brake Overcharge")] [GetParticularString("SwitchPanel", "")] ControlBrakeOvercharge,
        [GetString("Control Initialize Brakes")] [GetParticularString("SwitchPanel", "")] ControlInitializeBrakes,
        [GetString("Control Handbrake Full")] [GetParticularString("SwitchPanel", "Handbrake")] ControlHandbrakeFull,
        [GetString("Control Handbrake None")] [GetParticularString("SwitchPanel", "")] ControlHandbrakeNone,
        [GetString("Control Odometer Show/Hide")] [GetParticularString("SwitchPanel", "")] ControlOdoMeterShowHide,
        [GetString("Control Odometer Reset")] [GetParticularString("SwitchPanel", "")] ControlOdoMeterReset,
        [GetString("Control Odometer Direction")] [GetParticularString("SwitchPanel", "")] ControlOdoMeterDirection,
        [GetString("Control Retainers On")] [GetParticularString("SwitchPanel", "")] ControlRetainersOn,
        [GetString("Control Retainers Off")] [GetParticularString("SwitchPanel", "")] ControlRetainersOff,
        [GetString("Control Brake Hose Connect")] [GetParticularString("SwitchPanel", "Brake hose")] ControlBrakeHoseConnect,
        [GetString("Control Brake Hose Disconnect")] [GetParticularString("SwitchPanel", "")] ControlBrakeHoseDisconnect,
        [GetString("Control Alerter")] [GetParticularString("SwitchPanel", "Alerter")] ControlAlerter,
        [GetString("Control Emergency Push Button")] [GetParticularString("SwitchPanel", "Emergency")] ControlEmergencyPushButton,
        [GetString("Control EOT Emergency Brake")] [GetParticularString("SwitchPanel", "")] ControlEOTEmergencyBrake,
        [GetString("Control Sander")] [GetParticularString("SwitchPanel", "Sander")] ControlSander,
        [GetString("Control Sander Toggle")] [GetParticularString("SwitchPanel", "")] ControlSanderToggle,
        [GetString("Control Wiper")] [GetParticularString("SwitchPanel", "Wiper")] ControlWiper,
        [GetString("Control Horn")] [GetParticularString("SwitchPanel", "")] ControlHorn,
        [GetString("Control Bell")] [GetParticularString("SwitchPanel", "")] ControlBell,
        [GetString("Control Bell Toggle")] [GetParticularString("SwitchPanel", "")] ControlBellToggle,
        [GetString("Control Door Left")] [GetParticularString("SwitchPanel", "Door Left")] ControlDoorLeft,
        [GetString("Control Door Right")] [GetParticularString("SwitchPanel", "Door Right")] ControlDoorRight,
        [GetString("Control Mirror")] [GetParticularString("SwitchPanel", "")] ControlMirror,
        [GetString("Control Light")] [GetParticularString("SwitchPanel", "Light")] ControlLight,
        [GetString("Control Pantograph 1")] [GetParticularString("SwitchPanel", "Pantograph 1")] ControlPantograph1,
        [GetString("Control Pantograph 2")] [GetParticularString("SwitchPanel", "Pantograph 2")] ControlPantograph2,
        [GetString("Control Pantograph 3")] [GetParticularString("SwitchPanel", "")] ControlPantograph3,
        [GetString("Control Pantograph 4")] [GetParticularString("SwitchPanel", "")] ControlPantograph4,
        [GetString("Control Battery Close")] [GetParticularString("SwitchPanel", "Battery Switch")] ControlBatterySwitchClose,
        [GetString("Control Battery Open")] [GetParticularString("SwitchPanel", "")] ControlBatterySwitchOpen,
        [GetString("Control Master Key")] [GetParticularString("SwitchPanel", "Master Key")] ControlMasterKey,
        [GetString("Control Service Retention")] [GetParticularString("SwitchPanel", "")] ControlServiceRetention,
        [GetString("Control Service Retention Cancellation")] [GetParticularString("SwitchPanel", "")] ControlServiceRetentionCancellation,
        [GetString("Control Circuit Breaker Closing Order")] [GetParticularString("SwitchPanel", "Circuit Breaker")] ControlCircuitBreakerClosingOrder,
        [GetString("Control Circuit Breaker Opening Order")] [GetParticularString("SwitchPanel", "")] ControlCircuitBreakerOpeningOrder,
        [GetString("Control Circuit Breaker Closing Authorization")] [GetParticularString("SwitchPanel", "")] ControlCircuitBreakerClosingAuthorization,
        [GetString("Control Traction Cut-Off Relay Closing Order")] [GetParticularString("SwitchPanel", "Traction Cut-Off")] ControlTractionCutOffRelayClosingOrder,
        [GetString("Control Traction Cut-Off Relay Opening Order")] [GetParticularString("SwitchPanel", "")] ControlTractionCutOffRelayOpeningOrder,
        [GetString("Control Traction Cut-Off Relay Closing Authorization")] [GetParticularString("SwitchPanel", "")] ControlTractionCutOffRelayClosingAuthorization,
        [GetString("Control Electric Train Supply")] [GetParticularString("SwitchPanel", "")] ControlElectricTrainSupply,
        [GetString("Control Diesel Player")] [GetParticularString("SwitchPanel", "Diesel Player")] ControlDieselPlayer,
        [GetString("Control Diesel Helper")] [GetParticularString("SwitchPanel", "Diesel Helper")] ControlDieselHelper,
        [GetString("Control Headlight Increase")] [GetParticularString("SwitchPanel", "Front light")] ControlHeadlightIncrease,
        [GetString("Control Headlight Decrease")] [GetParticularString("SwitchPanel", "")] ControlHeadlightDecrease,
        [GetString("Control Injector 1 Increase")] [GetParticularString("SwitchPanel", "")] ControlInjector1Increase,
        [GetString("Control Injector 1 Decrease")] [GetParticularString("SwitchPanel", "")] ControlInjector1Decrease,
        [GetString("Control Injector 1")] [GetParticularString("SwitchPanel", "")] ControlInjector1,
        [GetString("Control Injector 2 Increase")] [GetParticularString("SwitchPanel", "")] ControlInjector2Increase,
        [GetString("Control Injector 2 Decrease")] [GetParticularString("SwitchPanel", "")] ControlInjector2Decrease,
        [GetString("Control Injector 2")] [GetParticularString("SwitchPanel", "")] ControlInjector2,
        [GetString("Control Blowdown Valve")] [GetParticularString("SwitchPanel", "")] ControlBlowdownValve,
        [GetString("Control Blower Increase")] [GetParticularString("SwitchPanel", "")] ControlBlowerIncrease,
        [GetString("Control Blower Decrease")] [GetParticularString("SwitchPanel", "")] ControlBlowerDecrease,
        [GetString("Control Steam Heat Increase")] [GetParticularString("SwitchPanel", "")] ControlSteamHeatIncrease,
        [GetString("Control Steam Heat Decrease")] [GetParticularString("SwitchPanel", "")] ControlSteamHeatDecrease,
        [GetString("Control Damper Increase")] [GetParticularString("SwitchPanel", "")] ControlDamperIncrease,
        [GetString("Control Damper Decrease")] [GetParticularString("SwitchPanel", "")] ControlDamperDecrease,
        [GetString("Control Firebox Open")] [GetParticularString("SwitchPanel", "")] ControlFireboxOpen,
        [GetString("Control Firebox Close")] [GetParticularString("SwitchPanel", "")] ControlFireboxClose,
        [GetString("Control Firing Rate Increase")] [GetParticularString("SwitchPanel", "")] ControlFiringRateIncrease,
        [GetString("Control Firing Rate Decrease")] [GetParticularString("SwitchPanel", "")] ControlFiringRateDecrease,
        [GetString("Control Fire Shovel Full")] [GetParticularString("SwitchPanel", "")] ControlFireShovelFull,
        [GetString("Control Cylinder Cocks")] [GetParticularString("SwitchPanel", "")] ControlCylinderCocks,
        [GetString("Control Large Ejector Increase")] [GetParticularString("SwitchPanel", "")] ControlLargeEjectorIncrease,
        [GetString("Control Large Ejector Decrease")] [GetParticularString("SwitchPanel", "")] ControlLargeEjectorDecrease,
        [GetString("Control Small Ejector Increase")] [GetParticularString("SwitchPanel", "")] ControlSmallEjectorIncrease,
        [GetString("Control Small Ejector Decrease")] [GetParticularString("SwitchPanel", "")] ControlSmallEjectorDecrease,
        [GetString("Control Vacuum Exhauster")] [GetParticularString("SwitchPanel", "")] ControlVacuumExhausterPressed,
        [GetString("Control Cylinder Compound")] [GetParticularString("SwitchPanel", "")] ControlCylinderCompound,
        [GetString("Control Firing")] [GetParticularString("SwitchPanel", "")] ControlFiring,
        [GetString("Control Refill")] [GetParticularString("SwitchPanel", "")] ControlRefill,
        [GetString("Control Discrete Unload")] [GetParticularString("SwitchPanel", "")] ControlDiscreteUnload,
        [GetString("Control Water Scoop")] [GetParticularString("SwitchPanel", "")] ControlWaterScoop,
        [GetString("Control ImmediateRefill")] [GetParticularString("SwitchPanel", "")] ControlImmediateRefill,
        [GetString("Control Turntable Clockwise")] [GetParticularString("SwitchPanel", "")] ControlTurntableClockwise,
        [GetString("Control Turntable Counterclockwise")] [GetParticularString("SwitchPanel", "")] ControlTurntableCounterclockwise,
        [GetString("Control Generic Item 1")] [GetParticularString("SwitchPanel", "")] ControlGenericItem1,
        [GetString("Control Generic Item 2")] [GetParticularString("SwitchPanel", "")] ControlGenericItem2,
        [GetString("Control TCS Generic 1")] [GetParticularString("SwitchPanel", "")] ControlTCSGeneric1,
        [GetString("Control TCS Generic 2")] [GetParticularString("SwitchPanel", "")] ControlTCSGeneric2,
        [GetString("Control Cab Radio")] [GetParticularString("SwitchPanel", "")] ControlCabRadio,
        [GetString("Control AI Fire On")] [GetParticularString("SwitchPanel", "")] ControlAIFireOn,
        [GetString("Control AI Fire Off")] [GetParticularString("SwitchPanel", "")] ControlAIFireOff,
        [GetString("Control AI Fire Reset")] [GetParticularString("SwitchPanel", "")] ControlAIFireReset,

        // Cruise Control
        [GetString("Control Speed Regulator Mode Increase")] [GetParticularString("SwitchPanel", "")] ControlSpeedRegulatorModeIncrease,
        [GetString("Control Speed Regulator Mode Descrease")] [GetParticularString("SwitchPanel", "")] ControlSpeedRegulatorModeDecrease,
        [GetString("Control Selected Speed Increase")] [GetParticularString("SwitchPanel", "")] ControlSpeedRegulatorSelectedSpeedIncrease,
        [GetString("Control Selected Speed Decrease")] [GetParticularString("SwitchPanel", "")] ControlSpeedRegulatorSelectedSpeedDecrease,
        [GetString("Control Speed Regulator Max Acceleration Increase")] [GetParticularString("SwitchPanel", "")] ControlSpeedRegulatorMaxAccelerationIncrease,
        [GetString("Control Speed Regulator Max Acceleration Decrease")] [GetParticularString("SwitchPanel", "")] ControlSpeedRegulatorMaxAccelerationDecrease,
        [GetString("Control Number Of Axles Increase")] [GetParticularString("SwitchPanel", "")] ControlNumberOfAxlesIncrease,
        [GetString("Control Number Of Axles Decrease")] [GetParticularString("SwitchPanel", "")] ControlNumberOfAxlesDecrease,
        [GetString("Control Restricted Speed Zone Active")] [GetParticularString("SwitchPanel", "")] ControlRestrictedSpeedZoneActive,
        [GetString("Control Cruise Control Mode Increase")] [GetParticularString("SwitchPanel", "")] ControlCruiseControlModeIncrease,
        [GetString("Control Cruise Control Mode Decrease")] [GetParticularString("SwitchPanel", "")] ControlCruiseControlModeDecrease,
        [GetString("Control Train Type Change (Passenger/Cargo)")] [GetParticularString("SwitchPanel", "")] ControlTrainTypePaxCargo,
        [GetString("Control Selected Speed To Zero")] [GetParticularString("SwitchPanel", "")] ControlSpeedRegulatorSelectedSpeedToZero,
        //Distributed power
        [GetString("Control DP Move To Front")] [GetParticularString("SwitchPanel", "")] ControlDPMoveToFront,
        [GetString("Control DP Move To Back")] [GetParticularString("SwitchPanel", "")] ControlDPMoveToBack,
        [GetString("Control DP Traction")] [GetParticularString("SwitchPanel", "")] ControlDPTraction,
        [GetString("Control DP Idle")] [GetParticularString("SwitchPanel", "")] ControlDPIdle,
        [GetString("Control DP Brake")] [GetParticularString("SwitchPanel", "")] ControlDPBrake,
        [GetString("Control DP More")] [GetParticularString("SwitchPanel", "")] ControlDPMore,
        [GetString("Control DP Less")] [GetParticularString("SwitchPanel", "")] ControlDPLess,
    }
}
