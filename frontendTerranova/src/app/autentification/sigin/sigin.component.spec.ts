import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SiginComponent } from './sigin.component';

describe('SiginComponent', () => {
  let component: SiginComponent;
  let fixture: ComponentFixture<SiginComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SiginComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SiginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
