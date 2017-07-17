namespace ContosoUniversityCore.IntegrationTests
{
    using System;
    using Construktion;
    using Xunit.Sdk;
    using System.Collections.Generic;
    using System.Reflection;

    public class ConstruktionData : DataAttribute
    {
        private readonly Construktion _construktion;

        public ConstruktionData()
        {
            _construktion = new Construktion().With(x =>
            {
                x.OmitIds();
                x.OmitVirtualProperties();
                x.OmitProperties(p => p.Name.EndsWith("ID"), typeof(int), typeof(int?));
                x.OmitProperties(p => p.Name.Equals("RowVersion"), typeof(byte[]));
                x.ConstructPropertyUsing(p => p.Name.Equals("Credits"), () => new Random().Next(0, 6));
                x.ConstructPropertyUsing(p => p.Name.Equals("HireDate"), () => DateTime.Today);
                x.ConstructPropertyUsing(p => p.Name.Equals("StartDate"), () => DateTime.Today);
            });
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            var parameters = new List<object>();

            foreach (var paramInfo in testMethod.GetParameters())
            {
                var result = _construktion.Construct(paramInfo);

                parameters.Add(result);
            }

            return new[] { parameters.ToArray() };
        }
    }
}
