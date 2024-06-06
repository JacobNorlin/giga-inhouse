import React from "react";



export type UserInfo = {
  name: string;
  userId: string;
  steamId?: string;
};

type AuthContextState = {
  user: UserInfo;
};

export const AuthContext = React.createContext<AuthContextState | null>(null);

type AuthContextProviderProps = React.PropsWithChildren<{
  user: UserInfo;
}>;

export function AuthContextProvider({
  user,
  children,
}: AuthContextProviderProps) {
  const memoizedState = React.useMemo(() => {
    return {
      user: user,
    };
  }, [user]);

  return (
    <AuthContext.Provider value={memoizedState}>
      {children}
    </AuthContext.Provider>
  );
}
