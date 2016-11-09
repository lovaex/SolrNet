using System;
using System.Collections.Generic;
using SolrNet.Mapping.Validation;
using SolrNet.Schema;
using SolrNet.Tests.Utils;

namespace SolrNet.Tests.Mocks {
    public class MMappingValidator : IMappingValidator {
        public MFunc<Type, SolrSchema, IEnumerable<ValidationResult>> enumerate;

        public IEnumerable<ValidationResult> EnumerateValidationResults(Type documentType, SolrSchema schema) {
            return enumerate.Invoke(documentType, schema);
        }
    }
}