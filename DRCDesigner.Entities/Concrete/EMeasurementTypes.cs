using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DRCDesigner.Entities.Concrete
{
    public enum EMeasurementTypes
    {
        /// <summary>
        /// Acceleration
        /// </summary>
        [Display(Name = "Acceleration")]
        Acceleration = 0,
        /// <summary>
        /// AmountOfSubstance
        /// </summary>
        [Display(Name = "AmountOfSubstance")]
        AmountOfSubstance = 1,
        /// <summary>
        /// AmplitudeRatio
        /// </summary>
        [Display(Name = "AmplitudeRatio")]
        AmplitudeRatio = 2,
        /// <summary>
        /// Angle
        /// </summary>
        [Display(Name = "Angle")]
        Angle = 3,
        /// <summary>
        /// ApparentEnergy
        /// </summary>
        [Display(Name = "ApparentEnergy")]
        ApparentEnergy = 4,
        /// <summary>
        /// ApparentPower
        /// </summary>
        [Display(Name = "ApparentPower ")]
        ApparentPower = 5,
        /// <summary>
        /// Area
        /// </summary>
        [Display(Name = "Area")]
        Area = 6,
        /// <summary>
        /// AreaDensity
        /// </summary>
        [Display(Name = "AreaDensity")]
        AreaDensity = 7,
        /// <summary>
        /// AreaMomentOfInertia
        /// </summary>
        [Display(Name = "AreaMomentOfInertia")]
        AreaMomentOfInertia = 8,
        /// <summary>
        /// BitRate
        /// </summary>
        [Display(Name = "BitRate")]
        BitRate = 9,
        /// <summary>
        /// BrakeSpecificFuelConsumption
        /// </summary>
        [Display(Name = "BrakeSpecificFuelConsumption")]
        BrakeSpecificFuelConsumption=10,
        /// <summary>
        /// Capacitance
        /// </summary>
        [Display(Name = "Capacitance")]
        Capacitance = 11,
        /// <summary>
        /// CoefficientOfThermalExpansion
        /// </summary>
        [Display(Name = "CoefficientOfThermalExpansion")]
        CoefficientOfThermalExpansion = 12,
        /// <summary>
        /// Density
        /// </summary>
        [Display(Name = "Density")]
        Density = 13,
        /// <summary>
        /// Duration
        /// </summary>
        [Display(Name = "Duration")]
        Duration = 14,
        /// <summary>
        /// DynamicViscosity
        /// </summary>
        [Display(Name = "DynamicViscosity")]
        DynamicViscosity = 15,
        /// <summary>
        /// ElectricAdmittance
        /// </summary>
        [Display(Name = "ElectricAdmittance")]
        ElectricAdmittance = 16,
        /// <summary>
        /// ElectricCharge
        /// </summary>
        [Display(Name = "ElectricCharge")]
        ElectricCharge = 17,
        /// <summary>
        /// ElectricChargeDensity
        /// </summary>
        [Display(Name = "ElectricChargeDensity")]
        ElectricChargeDensity = 18,
        /// <summary>
        /// ElectricConductance
        /// </summary>
        [Display(Name = "ElectricConductance")]
        ElectricConductance = 19,
        /// <summary>
        /// ElectricConductivity
        /// </summary>
        [Display(Name = "ElectricConductivity")]
        ElectricConductivity = 20,
        /// <summary>
        /// ElectricCurrent
        /// </summary>
        [Display(Name = "ElectricCurrent")]
        ElectricCurrent=21,
        /// <summary>
        /// ElectricCurrentDensity
        /// </summary>
        [Display(Name = "ElectricCurrentDensity")]
        ElectricCurrentDensity=22,
        /// <summary>
        /// ElectricCurrentGradient
        /// </summary>
        [Display(Name = "ElectricCurrentGradient")]
        ElectricCurrentGradient=23,
        /// <summary>
        /// ElectricField
        /// </summary>
        [Display(Name = "ElectricField")]
        ElectricField=24,
        /// <summary>
        /// ElectricInductance
        /// </summary>
        [Display(Name = "ElectricInductance")]
        ElectricInductance=25,
        /// <summary>
        /// ElectricPotential
        /// </summary>
        [Display(Name = "ElectricPotential")]
        ElectricPotential=26,
        /// <summary>
        /// ElectricPotentialAc
        /// </summary>
        [Display(Name = "ElectricPotentialAc")]
        ElectricPotentialAc=27,
        /// <summary>
        /// ElectricPotentialDc
        /// </summary>
        [Display(Name = "ElectricPotentialDc")]
        ElectricPotentialDc=28,
        /// <summary>
        /// ElectricResistance
        /// </summary>
        [Display(Name = "ElectricResistance")]
        ElectricResistance=29,
        /// <summary>
        /// ElectricResistivity
        /// </summary>
        [Display(Name = "ElectricResistivity")]
        ElectricResistivity=30,
        /// <summary>
        /// ElectricSurfaceChargeDensity
        /// </summary>
        [Display(Name = "ElectricSurfaceChargeDensity")]
        ElectricSurfaceChargeDensity=31,
        /// <summary>
        /// Energy
        /// </summary>
        [Display(Name = "Energy")]
        Energy=32,
        /// <summary>
        /// Entropy
        /// </summary>
        [Display(Name = "Entropy")]
        Entropy=33,
        /// <summary>
        /// Force
        /// </summary>
        [Display(Name = "Force")]
        Force=34,
        /// <summary>
        /// ForceChangeRate
        /// </summary>
        [Display(Name = "ForceChangeRate")]
        ForceChangeRate=35,
        /// <summary>
        /// ForcePerLength
        /// </summary>
        [Display(Name = "ForcePerLength")]
        ForcePerLength=36,
        /// <summary>
        /// Frequency
        /// </summary>
        [Display(Name = "Frequency")]
        Frequency=37,
        /// <summary>
        /// HeatFlux
        /// </summary>
        [Display(Name = "HeatFlux")]
        HeatFlux=38,
        /// <summary>
        /// HeatTransferCoefficient
        /// </summary>
        [Display(Name = "HeatTransferCoefficient")]
        HeatTransferCoefficient=39,
        /// <summary>
        /// Illuminance
        /// </summary>
        [Display(Name = "Illuminance")]
        Illuminance=40,
        /// <summary>
        /// Information
        /// </summary>
        [Display(Name = "Information")]
        Information=41,
        /// <summary>
        /// Irradiance
        /// </summary>
        [Display(Name = "Irradiance")]
        Irradiance=42,
        /// <summary>
        /// Irradiation
        /// </summary>
        [Display(Name = "Irradiation")]
        Irradiation=43,
        /// <summary>
        /// KinematicViscosity
        /// </summary>
        [Display(Name = "KinematicViscosity")]
        KinematicViscosity=44,
        /// <summary>
        /// LapseRate
        /// </summary>
        [Display(Name = "LapseRate")]
        LapseRate=45,
        /// <summary>
        /// Length
        /// </summary>
        [Display(Name = "Length")]
        Length=46,
        /// <summary>
        /// Level
        /// </summary>
        [Display(Name = "Level")]
        Level=47,
        /// <summary>
        /// LinearDensity
        /// </summary>
        [Display(Name = "LinearDensity")]
        LinearDensity=48,
        /// <summary>
        /// LuminousFlux
        /// </summary>
        [Display(Name = "LuminousFlux")]
        LuminousFlux=49,
        /// <summary>
        /// LuminousIntensity
        /// </summary>
        [Display(Name = "LuminousIntensity")]
        LuminousIntensity=50,
        /// <summary>
        /// MagneticField
        /// </summary>
        [Display(Name = "MagneticField")]
        MagneticField=51,
        /// <summary>
        /// MagneticFlux
        /// </summary>
        [Display(Name = "MagneticFlux")]
        MagneticFlux=52,
        /// <summary>
        /// Magnetization
        /// </summary>
        [Display(Name = "Magnetization")]
        Magnetization=53,
        /// <summary>
        /// Mass
        /// </summary>
        [Display(Name = "Mass")]
        Mass=54,
        /// <summary>
        /// MassConcentration
        /// </summary>
        [Display(Name = "MassConcentration")]
        MassConcentration=55,
        /// <summary>
        /// MassFlow
        /// </summary>
        [Display(Name = "MassFlow")]
        MassFlow=56,
        /// <summary>
        /// MassFlux
        /// </summary>
        [Display(Name = "MassFlux")]
        MassFlux=57,
        /// <summary>
        /// MassFraction
        /// </summary>
        [Display(Name = "MassFraction")]
        MassFraction=58,
        /// <summary>
        /// MassMomentOfInertia
        /// </summary>
        [Display(Name = "MassMomentOfInertia")]
        MassMomentOfInertia=59,
        /// <summary>
        /// MolarEnergy
        /// </summary>
        [Display(Name = "MolarEnergy")]
        MolarEnergy=60,
        /// <summary>
        /// MolarEntropy
        /// </summary>
        [Display(Name = "MolarEntropy")]
        MolarEntropy=61,
        /// <summary>
        /// Molarity
        /// </summary>
        [Display(Name = "Molarity")]
        Molarity=62,
        /// <summary>
        /// MolarMass
        /// </summary>
        [Display(Name = "MolarMass")]
        MolarMass=63,
        /// <summary>
        /// Permeability
        /// </summary>
        [Display(Name = "Permeability")]
        Permeability=64,
        /// <summary>
        /// Permittivity
        /// </summary>
        [Display(Name = "Permittivity")]
        Permittivity=65,
        /// <summary>
        /// Power
        /// </summary>
        [Display(Name = "Power")]
        Power=66,
        /// <summary>
        /// PowerDensity
        /// </summary>
        [Display(Name = "PowerDensity")]
        PowerDensity=67,
        /// <summary>
        /// PowerRatio
        /// </summary>
        [Display(Name = "PowerRatio")]
        PowerRatio=68,
        /// <summary>
        /// Pressure
        /// </summary>
        [Display(Name = "Pressure")]
        Pressure=69,
        /// <summary>
        /// PressureChangeRate
        /// </summary>
        [Display(Name = "PressureChangeRate")]
        PressureChangeRate=70,
        /// <summary>
        /// Ratio
        /// </summary>
        [Display(Name = "Ratio")]
        Ratio=71,
        /// <summary>
        /// ReactiveEnergy
        /// </summary>
        [Display(Name = "ReactiveEnergy")]
        ReactiveEnergy=72,
        /// <summary>
        /// ReactivePower
        /// </summary>
        [Display(Name = "ReactivePower")]
        ReactivePower=73,
        /// <summary>
        /// RotationalAcceleration
        /// </summary>
        [Display(Name = "RotationalAcceleration")]
        RotationalAcceleration=74,
        /// <summary>
        /// RotationalSpeed
        /// </summary>
        [Display(Name = "RotationalSpeed")]
        RotationalSpeed=75,
        /// <summary>
        /// RotationalStiffness
        /// </summary>
        [Display(Name = "RotationalStiffness")]
        RotationalStiffness=76,
        /// <summary>
        /// RotationalStiffnessPerLength
        /// </summary>
        [Display(Name = "RotationalStiffnessPerLength")]
        RotationalStiffnessPerLength=77,
        /// <summary>
        /// SolidAngle
        /// </summary>
        [Display(Name = "SolidAngle")]
        SolidAngle=78,
        /// <summary>
        /// SpecificEnergy
        /// </summary>
        [Display(Name = "SpecificEnergy")]
        SpecificEnergy=79,
        /// <summary>
        /// SpecificEntropy
        /// </summary>
        [Display(Name = "SpecificEntropy")]
        SpecificEntropy=80,
        /// <summary>
        /// SpecificVolume
        /// </summary>
        [Display(Name = "SpecificVolume")]
        SpecificVolume=81,
        /// <summary>
        /// SpecificWeight
        /// </summary>
        [Display(Name = "SpecificWeight")]
        SpecificWeight=82,
        /// <summary>
        /// Speed
        /// </summary>
        [Display(Name = "Speed")]
        Speed=83,
        /// <summary>
        /// Temperature
        /// </summary>
        [Display(Name = "Temperature")]
        Temperature=84,
        /// <summary>
        /// TemperatureChangeRate
        /// </summary>
        [Display(Name = "TemperatureChangeRate")]
        TemperatureChangeRate=85,
        /// <summary>
        /// TemperatureDelta
        /// </summary>
        [Display(Name = "TemperatureDelta")]
        TemperatureDelta=86,
        /// <summary>
        /// ThermalConductivity
        /// </summary>
        [Display(Name = "ThermalConductivity")]
        ThermalConductivity=87,
        /// <summary>
        /// ThermalResistance
        /// </summary>
        [Display(Name = "ThermalResistance")]
        ThermalResistance=88,
        /// <summary>
        /// Torque
        /// </summary>
        [Display(Name = "Torque")]
        Torque=89,
        /// <summary>
        /// VitaminA
        /// </summary>
        [Display(Name = "VitaminA")]
        VitaminA=90,
        /// <summary>
        /// Volume
        /// </summary>
        [Display(Name = "Volume")]
        Volume=91,
        /// <summary>
        /// VolumeConcentration
        /// </summary>
        [Display(Name = "VolumeConcentration")]
        VolumeConcentration=92,
        /// <summary>
        /// VolumeFlow
        /// </summary>
        [Display(Name = "VolumeFlow")]
        VolumeFlow=93,
        /// <summary>
        /// VolumePerLength
        /// </summary>
        [Display(Name = "VolumePerLength")]
        VolumePerLength=94
    }

}
