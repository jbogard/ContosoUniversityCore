namespace ContosoUniversityCore.Features.Student
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class StudentController : Controller
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ViewResult> Index(Index.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }

        public async Task<IActionResult> Details(Details.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new Create.Command());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Create.Command command)
        {
            _mediator.Send(command);

            return this.RedirectToActionJson(nameof(Index));
        }


        public async Task<IActionResult> Edit(Edit.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            await _mediator.SendAsync(command);

            return this.RedirectToActionJson(nameof(Index));
        }

        public async Task<IActionResult> Delete(Delete.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Delete.Command command)
        {
            await _mediator.SendAsync(command);

            return this.RedirectToActionJson(nameof(Index));
        }
    }
}