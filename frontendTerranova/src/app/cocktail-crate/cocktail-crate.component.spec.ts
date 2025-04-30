import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CocktailCrateComponent } from './cocktail-crate.component';

describe('CocktailCrateComponent', () => {
  let component: CocktailCrateComponent;
  let fixture: ComponentFixture<CocktailCrateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CocktailCrateComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CocktailCrateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
