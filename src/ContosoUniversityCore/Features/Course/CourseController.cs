namespace ContosoUniversityCore.Features.Course
{
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class CourseController : Controller
    {
        private readonly IMediator _mediator;

        public CourseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> Index(Index.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }

        public async Task<ActionResult> Details(Details.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Create.Command command)
        {
            _mediator.Send(command);

            return null;
            //return this.RedirectToActionJson("Index");
        }

        public async Task<ActionResult> Edit(Edit.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Edit.Command command)
        {
            await _mediator.SendAsync(command);

            return null;
            //return this.RedirectToActionJson("Index");
        }

        public async Task<ActionResult> Delete(Delete.Query query)
        {
            var model = await _mediator.SendAsync(query);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Delete.Command command)
        {
            await _mediator.SendAsync(command);

            return null;
            //return this.RedirectToActionJson("Index");
        }
    }
}