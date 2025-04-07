export interface Cocktail {
    id: number; 
    name: string; 
    imageUrl: string;
    ingredients: string[]; 
    instructions: string; 
    glassType: string; 
    category: string; 
    alcoholic: boolean;
    createdBy: string;
}
