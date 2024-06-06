/* eslint-disable @typescript-eslint/no-explicit-any */
import { Alert, Loader } from "@mantine/core";
import { UseQueryResult } from "@tanstack/react-query";
import React from "react";

type Variadic<T> = [T, ...T[]];

type QueryData<T, Out extends any[] = []> = T extends [
  infer First,
  ...infer Tail
]
  ? First extends UseQueryResult<infer TData, any>
    ? QueryData<Tail, [...Out, TData]>
    : never
  : Out;
type QueryError<T, Out extends any[] = []> = T extends [
  infer First,
  ...infer Tail
]
  ? First extends UseQueryResult<any, infer TError>
    ? QueryData<Tail, [...Out, TError]>
    : never
  : Out;

type QueryWrapperProps<TStates extends Variadic<UseQueryResult<any, any>>> = {
  queryStates: TStates;
  children: (...states: QueryData<TStates>) => React.ReactNode;
  renderError?: (...errors: QueryError<TStates>) => React.ReactNode;
  renderLoading?: () => React.ReactNode;
};

export function QueryWrapper<
  TStates extends Variadic<UseQueryResult<any, any>>
>({
  queryStates,
  children,
  renderLoading,
  renderError,
}: QueryWrapperProps<TStates>) {
  const isLoading = queryStates.some((state) => state.status === "pending");
  const failingQueries = queryStates.filter(
    (state) => state.status === "error"
  );
  const isError = failingQueries.length > 0;

  if (isLoading) {
    if (renderLoading) {
      return renderLoading();
    }
    return <Loader />;
  }

  if (isError) {
    const errors = queryStates.map(
      (state) => state.error
    ) as QueryError<TStates>;
    if (renderError) {
      return renderError(...errors);
    }

    return (
      <Alert title="Something went wrong :)">Wonder what is wrong? ;^)</Alert>
    );
  }

  const datas: QueryData<TStates> = queryStates.map(
    (state) => state.data
  ) as QueryData<TStates>;
  return children(...datas);
}
