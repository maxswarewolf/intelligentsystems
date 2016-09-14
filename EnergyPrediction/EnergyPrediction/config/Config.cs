//
// MIT LICENSE
//
// GeneticLinker.cs
//
// Author:
//       Katie Clark, Sean Grinter, Adrian Pellegrino <Energy Prediction>
//
// Copyright (c) 2016 Katie Clark, Sean Grinter, Adrian Pellegrino
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Reinsertions;
using GeneticSharp.Domain.Terminations;

namespace EnergyPrediction
{
    public class Config
    {
        public Config()
        {
            SolutionOptions = new List<string>() { "Genetic Algorithm", "Genetic Programming" };

            CrossoversAlgo = new List<string>() { "Uniform", "One Point", "Two Point", "Three Parent" };
            CrossoversProg = new List<string>() { "PLACE HOLDER" };

            Fitness = new List<string>() { "Error Sum" };

            MutationAlgo = new List<string>() { "Uniform", "Twors", "Reverse Sequence" };
            MutationProg = new List<string>() { "Uniform", "Branch", "Reverse Sequence" };

            Selection = new List<string>() { "Elite", "Stochastic", "Inverse Elite" };

            Reinsertion = new List<string>() { "PLACE HOLDER" };

            GeneratorLinks = new Dictionary<string, List<string>>() {
                { "ACT", new List<string>() {"ROYALLA1"}},
                { "VIC", new List<string>() { "BAPS", "MCKAY1", "DARTM1", "EILDON1", "EILDON2", "MACARTH1", "OAKLAND1", "AGLSOM", "WKIEWA1", "WKIEWA2", "LYA1", "LYA2", "LYA3", "LYA4", "BALDHWF1", "BDL01", "BDL02", "CBWF1", "SHEP1", "TATURA01", "JLA01", "JLA02", "JLA03", "JLA04", "JLB01", "JLB02", "JLB03", "NPS", "BROADMDW", "BROOKLYN", "CLAYTON", "CORIO1", "SVALE1", "CODRNGTON", "YAMBUKWF", "MORNW", "WYNDW", "YWPS1", "YWPS2", "YWPS3", "YWPS4", "BBASEHOS", "HUMEV", "HWPS1", "HWPS2", "HWPS3", "HWPS4", "HWPS5", "HWPS6", "HWPS7", "HWPS8", "LOYYB1", "LOYYB2", "HALLAMRD1", "WOLLERT1", "MLWF1", "MERSER01", "TGNSS1", "LONGFORD", "MORTLK11", "MORTLK12", "CHALLHWF", "EILDON3", "GLENMAG1", "WILLHOW1", "PORTWF", "HLMSEW01", "WONWP", "WAUBRAWF", "HEPWIND1", "LNGS1", "LNGS2", "MURRAY", "VPGS1", "VPGS2", "VPGS3", "VPGS4", "VPGS5", "VPGS6", "TOORAWF" } },
                { "TAS", new List<string>() { "BBTHREE1", "BBTHREE2", "BBTHREE3", "TVCC201", "TVPP104", "BASTYAN", "BBDISEL1", "BUTLERSG", "LI_WY_CA", "CATAGUN1", "CETHANA", "CLUNY", "DEVILS_G", "FISHER", "GORDON", "GEORGTN1", "JBUTTERS", "LK_ECHO", "LEM_WIL", "MACKNTSH", "MEADOWBK", "MEADOWB2", "SSELR1", "PALOONA", "POAT110", "POAT220", "PORTLAT1", "QUERIVE1", "REECE1", "REECE2", "REPULSE", "ROWALLAN", "TARRALEA", "TREVALLN", "TRIBUTE", "TUNGATIN", "WOOLNTH1", "REMOUNT", "MIDLDPS1"} },
                { "SA" , new List<string>() { "HALLWF1", "HALLWF2", "NBHWF1", "BLUFF1", "WPWF", "TORRA1", "TORRA2", "TORRA3", "TORRA4", "TORRB1", "TORRB2", "TORRB3", "TORRB4", "BOLIVAR1", "CATHROCK", "PEDLER1", "WING1_1", "WING2_1", "AGLHAL", "BLULAKE1", "TATIARA1", "NPS1", "NPS2", "PLAYB-AG", "TERMSTOR", "LKBONNY1", "LKBONNY2", "LKBONNY3", "ANGAST1", "LONSDALE", "PTSTAN1", "MTMILLAR", "LADBROK1", "LADBROK2", "OSB-AG", "QPS1", "QPS2", "QPS3", "QPS4", "QPS5", "CLEMGPWF", "PPCCGT", "SNOWTWN1", "SNOWSTH1", "STARHLWF", "DRYCGT1", "DRYCGT2", "DRYCGT3", "MINTARO", "POR01", "POR03", "SNUG1", "WATERLWF"} },
                { "QLD", new List<string>() { "ICSM", "MORANBAH", "EDLRGNRD", "SUNCOAST", "YABULU", "YABULU2", "BRAEMAR1", "BRAEMAR2", "BRAEMAR3", "CPP_3", "CPP_4", "CALL_A_4", "CALL_B_1", "CALL_B_2", "GSTONE1", "GSTONE2", "GSTONE3", "GSTONE4", "GSTONE5", "GSTONE6", "KPP_1", "W/HOE#1", "W/HOE#2", "GERMCRK", "BPLANDF1", "MBAHNTH", "BARCALDN", "OAKEY1", "OAKEY2", "RPCG", "MPP_1", "MPP_2", "BRAEMAR5", "BRAEMAR6", "BRAEMAR7", "DAANDINE", "DDPS1", "MSTUART1", "MSTUART2", "MSTUART3", "OAKYCREK", "ROCHEDAL", "ROMA_7", "ROMA_8", "WHIT1", "CPSA", "YARWUN_1", "STHBKTEC", "BARRON-2", "KAREEYA1", "KAREEYA2", "KAREEYA3", "KAREEYA4", "KAREEYA5", "MACKAYGT", "STAN-1", "STAN-2", "STAN-3", "STAN-4", "SWAN_E", "TNPS1", "TARONG#1", "TARONG#2", "TARONG#3", "TARONG#4", "TITREE", "INVICTA", "WHILL1"} },
                { "NSW", new List<string>() { "BROKENH1", "BDONGHYD", "COPTNHYD", "GLBWNHYD", "KINCUM1", "NYNGAN1", "PINDARI", "AGLNOW1", "WOYWOY1", "BW01", "BW02", "BW03", "BW04", "HVGTS", "LD01", "LD02", "LD03", "LD04", "GRANGEAV", "BOCORWF1", "BWTR1", "CONDONG1", "BROWNMT", "EASTCRK", "GRANGEAV", "JACKSGUL", "LUCAS2S2", "LUCASHGT", "OAKY2", "MP1", "MP2", "TALWA1", "WDLNGN01", "BANKSPT1", "NINEWIL1", "STGEORG1", "WESTILL1", "WESTCBT1", "TERALBA", "GB01", "BURRIN", "HUMENSW", "KEEPIT", "GUNNING1", "WYANGALA", "WYANGALB", "CESF1", "CAPTL_WF", "AWABAREF", "HEZ", "SITHE01", "MOREESF1", "GULLRWF1", "SHGEN", "CULLRGWF", "ER01", "ER02", "ER03", "ER04", "ERGT01", "URANQ11", "URANQ12", "URANQ13", "URANQ14", "THEDROP1", "WILGAPK", "WILGB01", "BLOWERNG", "CG1", "CG2", "CG3", "CG4", "GUTHEGA", "JNDABNE1", "JOUNAMA1", "UPPTUMUT", "TUMUT3", "VP5", "VP6", "TARALGA1", "WOODLWN1"} }
            };

            DefaultPopulation = 200;
            DefaultMutationProbility = 0.4f;
            DefaultCrossoverProbility = 0.7f;
            DefaultTerminationTimeMinutes = 1;
            DefaultTerminationFitnessThreshold = 0;
            DefaultTerminationGenerationThreshold = 1000000;
        }

        #region Default Options
        public List<string> SolutionOptions { get; private set; }
        public List<string> CrossoversAlgo { get; private set; }
        public List<string> CrossoversProg { get; private set; }
        public List<string> Fitness { get; private set; }
        public List<string> MutationAlgo { get; private set; }
        public List<string> MutationProg { get; private set; }
        public List<string> Selection { get; private set; }
        public List<string> Reinsertion { get; private set; }
        public IDictionary<string, List<string>> GeneratorLinks { get; private set; }
        #endregion

        #region Option Choice
        public int DefaultTerminationTimeMinutes { get; private set; }
        public int DefaultTerminationGenerationThreshold { get; private set; }
        public double DefaultTerminationFitnessThreshold { get; private set; }
        public double DefaultCrossoverProbility { get; private set; }
        public double DefaultMutationProbility { get; private set; }
        public int DefaultPopulation { get; private set; }
        #endregion
    }
}

