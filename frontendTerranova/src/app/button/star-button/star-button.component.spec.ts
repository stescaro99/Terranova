import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StarButtonComponent } from './star-button.component';

describe('StarButtonComponent', () => {
  let component: StarButtonComponent;
  let fixture: ComponentFixture<StarButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StarButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StarButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
