using Ervilhinha.Models;

namespace Ervilhinha.Services
{
    /// <summary>
    /// Serviço de Cálculo Inteligente de Custos do Bebé
    /// Baseado em dados reais de Portugal (2024-2025)
    /// </summary>
    public class BabyCostCalculatorService
    {
        // Custos base por fase (valores médios em Portugal)
        private readonly Dictionary<LifestyleLevel, decimal> _pregnancyBaseCosts = new()
        {
            { LifestyleLevel.Economico, 800m },
            { LifestyleLevel.Moderado, 1500m },
            { LifestyleLevel.Confortavel, 2500m },
            { LifestyleLevel.Premium, 4000m }
        };

        private readonly Dictionary<BirthType, decimal> _birthCosts = new()
        {
            { BirthType.PublicoNormal, 50m },      // Apenas despesas extras no SNS
            { BirthType.PublicoCesariana, 100m },
            { BirthType.PrivadoNormal, 2500m },
            { BirthType.PrivadoCesariana, 3500m }
        };

        private readonly Dictionary<LifestyleLevel, decimal> _monthly0to6Costs = new()
        {
            { LifestyleLevel.Economico, 180m },    // Fraldas, produtos básicos
            { LifestyleLevel.Moderado, 320m },
            { LifestyleLevel.Confortavel, 550m },
            { LifestyleLevel.Premium, 850m }
        };

        private readonly Dictionary<LifestyleLevel, decimal> _monthly6to12Costs = new()
        {
            { LifestyleLevel.Economico, 250m },    // Inclui alimentação complementar
            { LifestyleLevel.Moderado, 420m },
            { LifestyleLevel.Confortavel, 680m },
            { LifestyleLevel.Premium, 1100m }
        };

        public BabyCostSimulator CalculateCosts(BabyCostSimulator simulator)
        {
            // 1. Custos de Gravidez
            simulator.EstimatedPregnancyCost = CalculatePregnancyCost(simulator);

            // 2. Custos de Nascimento
            simulator.EstimatedBirthCost = _birthCosts[simulator.ExpectedBirthType];

            // 3. Custos 0-6 meses
            simulator.Estimated0to6MonthsCost = Calculate0to6MonthsCost(simulator);

            // 4. Custos 6-12 meses
            simulator.Estimated6to12MonthsCost = Calculate6to12MonthsCost(simulator);

            // 5. Total do primeiro ano
            simulator.EstimatedTotalFirstYear = 
                simulator.EstimatedPregnancyCost + 
                simulator.EstimatedBirthCost + 
                simulator.Estimated0to6MonthsCost + 
                simulator.Estimated6to12MonthsCost;

            // 6. Impacto mensal médio
            simulator.EstimatedMonthlyImpact = CalculateMonthlyImpact(simulator);

            // 7. Gerar recomendações
            simulator.Recommendations = GenerateRecommendations(simulator);

            simulator.LastUpdatedDate = DateTime.UtcNow;

            return simulator;
        }

        private decimal CalculatePregnancyCost(BabyCostSimulator simulator)
        {
            var baseCost = _pregnancyBaseCosts[simulator.Lifestyle];

            // Ajustes baseados em preferências
            if (simulator.HasDonatedItems)
            {
                baseCost *= 0.7m; // 30% de desconto se tem items oferecidos
            }

            return baseCost;
        }

        private decimal Calculate0to6MonthsCost(BabyCostSimulator simulator)
        {
            var monthlyCost = _monthly0to6Costs[simulator.Lifestyle];

            // Ajuste para amamentação (reduz custos com leite)
            if (simulator.WillBreastfeed)
            {
                monthlyCost *= 0.85m; // 15% mais económico
            }

            // Ajuste por items oferecidos
            if (simulator.HasDonatedItems)
            {
                monthlyCost *= 0.8m; // 20% de desconto
            }

            // Custo total de 6 meses
            return monthlyCost * 6;
        }

        private decimal Calculate6to12MonthsCost(BabyCostSimulator simulator)
        {
            var monthlyCost = _monthly6to12Costs[simulator.Lifestyle];

            // Ajuste por items oferecidos
            if (simulator.HasDonatedItems)
            {
                monthlyCost *= 0.85m; // 15% de desconto
            }

            // Custo total de 6 meses (dos 6 aos 12)
            return monthlyCost * 6;
        }

        private decimal CalculateMonthlyImpact(BabyCostSimulator simulator)
        {
            // Média ponderada considerando todo o primeiro ano
            var totalMonths = 12m;
            var avg0to6 = simulator.Estimated0to6MonthsCost / 6;
            var avg6to12 = simulator.Estimated6to12MonthsCost / 6;
            
            return (avg0to6 + avg6to12) / 2;
        }

        private string GenerateRecommendations(BabyCostSimulator simulator)
        {
            var recommendations = new List<string>();

            // Recomendação baseada no rendimento
            var monthlyImpactPercentage = (simulator.EstimatedMonthlyImpact / simulator.MonthlyIncome) * 100;

            if (monthlyImpactPercentage > 30)
            {
                recommendations.Add("⚠️ O impacto mensal representa mais de 30% do rendimento. Considera ajustar o estilo de vida para 'Económico' ou procurar apoios.");
            }
            else if (monthlyImpactPercentage > 20)
            {
                recommendations.Add("💡 O impacto é significativo. Planeia um fundo de emergência de pelo menos 3 meses.");
            }
            else
            {
                recommendations.Add("✅ O impacto financeiro está equilibrado com o teu rendimento.");
            }

            // Recomendação sobre apoio governamental
            if (simulator.HasGovernmentSupport)
            {
                recommendations.Add("💰 Com o abono de família, o impacto real será menor. Verifica os valores atualizados no site da Segurança Social.");
            }
            else
            {
                recommendations.Add("📋 Não te esqueças de solicitar o abono de família após o nascimento - pode ajudar a reduzir custos.");
            }

            // Recomendação sobre amamentação
            if (!simulator.WillBreastfeed)
            {
                recommendations.Add("🍼 Os custos incluem leite em pó (~60-100€/mês). Se possível, a amamentação reduz significativamente este custo.");
            }

            // Recomendação sobre items doados
            if (!simulator.HasDonatedItems)
            {
                recommendations.Add("💡 Items de segunda mão ou oferecidos podem poupar 20-30% dos custos. Verifica grupos de pais no Facebook.");
            }

            // Recomendação sobre parto privado
            if (simulator.ExpectedBirthType == BirthType.PrivadoNormal || simulator.ExpectedBirthType == BirthType.PrivadoCesariana)
            {
                recommendations.Add("🏥 Parto privado tem custos elevados. Confirma se o teu seguro de saúde cobre parte das despesas.");
            }

            // Próximos custos com base nas semanas
            if (simulator.PregnancyWeeks < 28)
            {
                recommendations.Add("📅 Nos próximos 3 meses prepara o quarto e compra itens essenciais. Orçamento recomendado: 800-1200€.");
            }
            else if (simulator.PregnancyWeeks >= 28 && simulator.PregnancyWeeks < 37)
            {
                recommendations.Add("🎒 Já podes preparar a mala da maternidade e finalizar compras essenciais (roupa 0-3M, fraldas, produtos de higiene).");
            }

            return string.Join("\n", recommendations);
        }

        /// <summary>
        /// Calcula quanto falta para um objetivo de poupança
        /// </summary>
        public decimal CalculateSavingsGoal(decimal currentSavings, decimal targetAmount)
        {
            return Math.Max(0, targetAmount - currentSavings);
        }

        /// <summary>
        /// Estima quantos meses são necessários para atingir um objetivo
        /// </summary>
        public int CalculateMonthsToGoal(decimal currentSavings, decimal targetAmount, decimal monthlySavings)
        {
            if (monthlySavings <= 0) return 0;
            
            var remaining = targetAmount - currentSavings;
            if (remaining <= 0) return 0;

            return (int)Math.Ceiling(remaining / monthlySavings);
        }

        /// <summary>
        /// Recomenda valor mensal de poupança baseado no rendimento
        /// </summary>
        public decimal RecommendMonthlySavings(decimal monthlyIncome, decimal estimatedTotalCost, int monthsUntilBirth)
        {
            if (monthsUntilBirth <= 0) monthsUntilBirth = 1;

            // Ideal: poupar 10-15% do rendimento
            var idealSavings = monthlyIncome * 0.12m;

            // Necessário para cobrir custos estimados
            var requiredSavings = estimatedTotalCost / monthsUntilBirth;

            // Retorna o maior dos dois (mas sem exceder 25% do rendimento)
            var recommended = Math.Max(idealSavings, requiredSavings);
            var maxSavings = monthlyIncome * 0.25m;

            return Math.Min(recommended, maxSavings);
        }
    }
}
