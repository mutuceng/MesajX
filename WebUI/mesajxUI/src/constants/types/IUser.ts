export interface User {
    token: string;
    userId: string;
    refreshToken: string;
    expiresIn: number;
    TokenType: string;
}