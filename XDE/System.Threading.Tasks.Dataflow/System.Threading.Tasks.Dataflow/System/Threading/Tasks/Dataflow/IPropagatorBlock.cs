using System;

namespace System.Threading.Tasks.Dataflow
{
	/// <summary>Represents a dataflow block that is both a target for data and a source of data.</summary>
	/// <typeparam name="TInput">Specifies the type of data accepted by the <see cref="T:System.Threading.Tasks.Dataflow.IPropagatorBlock`2" />.This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
	/// <typeparam name="TOutput">Specifies the type of data supplied by the <see cref="T:System.Threading.Tasks.Dataflow.IPropagatorBlock`2" />.This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived. For more information about covariance and contravariance, see Covariance and Contravariance in Generics.</typeparam>
	// Token: 0x0200002F RID: 47
	public interface IPropagatorBlock<in TInput, out TOutput> : ITargetBlock<TInput>, IDataflowBlock, ISourceBlock<TOutput>
	{
	}
}
